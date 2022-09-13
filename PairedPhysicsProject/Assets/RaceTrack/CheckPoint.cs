using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool finishLine = false;

    public bool passed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!finishLine)
                LapTimer.instance.PassCheckPoint(this);
            else
                LapTimer.instance.CheckWinCondition();
        }
    }
}
