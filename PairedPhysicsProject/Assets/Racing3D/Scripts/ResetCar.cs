using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ResetCar : MonoBehaviour
{
    [SerializeField] GameObject car;
    [SerializeField] Transform resetPos;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void OnResetCar(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Debug.Log("Reset");
            rb.velocity = Vector3.zero;
            car.transform.position = resetPos.position;
            car.transform.rotation = resetPos.rotation;
        }
    }
}
