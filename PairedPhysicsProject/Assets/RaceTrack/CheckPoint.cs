using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool finishLine = false;

    public bool passed = false;
    public bool current = false;

    private void OnTriggerEnter(Collider other)
    {
        print("enter trigger");

        if (other.gameObject.CompareTag("Player"))
        {
            if (!finishLine)
                LapTimer.instance.PassCheckPoint(this);
            else
                LapTimer.instance.CheckWinCondition();
        }
    }
}
