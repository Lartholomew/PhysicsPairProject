using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTimer : MonoBehaviour
{
    public static LapTimer instance;
    public List<CheckPoint> checkPoints;

    public bool counting = false;
    public float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (counting)
            currentTime += Time.deltaTime;
    }

    public void StartTimer()
    {
        counting = true;
    }

    public void PassCheckPoint(CheckPoint checkPoint)
    {
        foreach(CheckPoint c in checkPoints)
        {
            c.current = false;
            if(c == checkPoint)
            {
                c.passed = true;
                c.current = true;
            }
        }
    }

    public void CheckWinCondition()
    {
        bool win = true;
        foreach(CheckPoint c in checkPoints)
        {
            if (!c.passed)
                win = false;
        }

        if (win)
        {
            counting = false;

            //bring up menu and freeze player movement here
        }
    }
}
