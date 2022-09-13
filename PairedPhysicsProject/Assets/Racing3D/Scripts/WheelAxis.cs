using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WheelAxis
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool isMotor;
    public bool isSteering;

    public void UpdateAntiRollBars(float antiRollStrength, Rigidbody carBody)
    {
        float leftBar = 1;
        float rightBar = 1;

        // measure the pressure on each side of the car
        WheelHit hit;
        bool leftGrounded = leftWheel.GetGroundHit(out hit);
        if(leftGrounded)
        {
            leftBar = (-leftWheel.transform.InverseTransformPoint(hit.point).y - leftWheel.radius) / leftWheel.suspensionDistance;
        }
        bool rightGrounded = rightWheel.GetGroundHit(out hit);
        if(rightGrounded)
        {
            rightBar = (-rightWheel.transform.InverseTransformPoint(hit.point).y - rightWheel.radius) / rightWheel.suspensionDistance;
        }
        // applying forces
        float antiRollFroce = (leftBar - rightBar) + antiRollStrength;
        if(leftGrounded)
        {
            carBody.AddForceAtPosition(leftWheel.transform.up * -antiRollFroce, leftWheel.transform.position);
        }
        if(rightGrounded)
        {
            carBody.AddForceAtPosition(rightWheel.transform.up * -antiRollFroce, rightWheel.transform.position);
        }
    }

    void UpdateWheelVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
            return;

        Transform visual = collider.transform.GetChild(0); // get the first child in the heirarchy
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        visual.transform.position = position;
        visual.transform.rotation = rotation * Quaternion.AngleAxis(90, Vector3.up); // correcting for inherent rotation in the model used in game
    }

    public void UpdateVisuals()
    {
        UpdateWheelVisuals(leftWheel);
        UpdateWheelVisuals(rightWheel);
    }
}
