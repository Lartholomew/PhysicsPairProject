using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
// TODO: known bug, car doesnt stop reversing after it starts reversing
public class RacingController : MonoBehaviour
{
    public enum CarState { Driving, Reverse, Brake, Parked, Nitro}


    public static RacingController instance;

    [HideInInspector]
    public Rigidbody rb;
    private float averageRPM = 0;
    private bool isMovingReverse = false;
    private CarState state;

    float VerticalInput = 0;
    float HorizontalInput = 0;
    bool isBraking = false;
    bool isNitro = false;
    public float nitroFuel = 0.0f;
    public TMP_Text nitroText;


    public WheelAxis[] axles;

    [Header("Engine Stats")]
    public float maxMotorTorque;
    float defaultMaxSpeed;
    public float maxSpeed;
    public float nitroStrength;
    public AnimationCurve motorCurve; // use for adjusting hte amount of torque applied over time

    [Header("Braking Stats")]
    public float normalDampening;
    public float brakesDampening; // strength of brakes

    public float brakesThreshold;
    public float parkingThreshold;

    [Header("Steering Stats")]
    public float maxSteeringAngle;
    public float minSteeringAngle;
    public AnimationCurve steeringAngleCurve;
    public float steeringWheelSpeed;

    public float antiRollStrength;


    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, -0.18f);
        defaultMaxSpeed = maxSpeed;
    }

    private void FixedUpdate()
    {
        UpdateCarState();
        UpdateControlls();
        UpdateUI();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        VerticalInput = move.y;
        HorizontalInput = move.x;
    }

    public void OnNitro(InputAction.CallbackContext context)
    {
        if(context.performed)
            isNitro = !isNitro;
            
    }

    public void OnBrake(InputAction.CallbackContext context)
    {
        if(context.performed)
         isBraking = !isBraking;
    }

    private void UpdateCarState()
    {
        if(isBraking)
        {
            state = CarState.Brake;
        }
        else if(averageRPM > parkingThreshold)
        {
            if(VerticalInput <= -brakesThreshold)
            {
                state = CarState.Brake;
            }
            else
            {
                state = CarState.Driving;
            }
        }
        else if(averageRPM < -parkingThreshold)
        {
            if(VerticalInput >= -brakesThreshold)
            {
                state = CarState.Brake;
            }
            else
            {
                state = CarState.Reverse;
            }
        }
        else
        {
            state = CarState.Parked;
        }
    }

    void UpdateControlls()
    {
        if(isNitro && nitroFuel > 0)
        {
           maxSpeed = Mathf.Lerp(maxSpeed, nitroStrength, Time.fixedDeltaTime);    
           maxSpeed = Mathf.Clamp(maxSpeed, defaultMaxSpeed, nitroStrength);
           nitroFuel--;
        }
        else if(isNitro && nitroFuel <= 0)
        {
            maxSpeed = Mathf.Lerp(maxSpeed, defaultMaxSpeed, Time.fixedDeltaTime);
        }
        else if (!isNitro)
        {
            maxSpeed = Mathf.Lerp(maxSpeed, defaultMaxSpeed, Time.fixedDeltaTime);
        }
        float speed = rb.velocity.magnitude;
        float speedPercent = Mathf.Abs(speed/maxSpeed);

        float totalRPM = 0;
        int nOfWheels = 0;
        foreach(WheelAxis axle in axles)
        {
            if(axle.isSteering)
            {
                float angle = minSteeringAngle + (steeringAngleCurve.Evaluate(speedPercent) * (maxSteeringAngle - minSteeringAngle));
                float steering = angle * HorizontalInput * Time.fixedDeltaTime * steeringWheelSpeed;
                axle.leftWheel.steerAngle = steering;
                axle.rightWheel.steerAngle = steering;
            }

            axle.leftWheel.wheelDampingRate = normalDampening;
            axle.rightWheel.wheelDampingRate = normalDampening;
            // braking and slowdown behavior
            if (state == CarState.Brake)
            {               
                axle.leftWheel.wheelDampingRate = brakesDampening;
                axle.rightWheel.wheelDampingRate = brakesDampening;
            }
            else if( axle.isMotor)
            {
                if(speed >= maxSpeed)
                {
                    axle.leftWheel.motorTorque = 0;
                    axle.rightWheel.motorTorque = 0;
                }
                else
                {
                    float horsePow = VerticalInput * Time.fixedDeltaTime * maxMotorTorque * motorCurve.Evaluate(speedPercent);
                    axle.leftWheel.motorTorque = horsePow;
                    axle.rightWheel.motorTorque = horsePow;
                }
            }

            // TODO: update AntiRolling and visuals for axis
            axle.UpdateAntiRollBars(antiRollStrength, rb);
            axle.UpdateVisuals();
            // acceleration

            totalRPM += axle.leftWheel.rpm + axle.rightWheel.rpm;
            nOfWheels += 2;

        }
        averageRPM = totalRPM / nOfWheels;
        isMovingReverse = averageRPM < -parkingThreshold; // are wheels spinning in reverse
    }

    void UpdateUI()
    {
        if (nitroFuel > 0)
            nitroText.text = "Boost: " + nitroFuel.ToString("###");
        else
            nitroText.text = "Boost: 0";
    }

    private void OnTriggerEnter(Collider other)
    {
        print("enter RC trigger");

        if (other.CompareTag("Death"))
        {
            print("Death here");
            LapTimer.instance.RespawnCar();
        }
    }
}
