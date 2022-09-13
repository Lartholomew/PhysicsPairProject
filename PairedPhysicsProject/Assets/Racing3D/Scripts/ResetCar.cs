using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ResetCar : MonoBehaviour
{
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void OnResetCar(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            LapTimer.instance.RespawnCar();
        }
    }
}
