using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LapTimer : MonoBehaviour
{
    public static LapTimer instance;
    public List<CheckPoint> checkPoints;
<<<<<<< Updated upstream
=======
    public GameObject endStateUIObj;

>>>>>>> Stashed changes
    public bool counting = false;
    public float currentTime;

    [SerializeField] TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (counting)
        {
            currentTime += Time.deltaTime;
            text.text = currentTime.ToString("##.##");
        }
            
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
        print("enter win");

        bool win = true;
        foreach(CheckPoint c in checkPoints)
        {
            if (!c.passed && !c.finishLine)
                win = false;
        }

        if (win)
        {
            print("win");
            counting = false;
            endStateUIObj.SetActive(true);
            Time.timeScale = 0.1f;
            //bring up menu and freeze player movement here

        }
    }

    public void RespawnCar()
    {
        foreach(CheckPoint checkPoint in checkPoints)
        {
            if (checkPoint.current)
            {
                RacingController.instance.transform.position = checkPoint.transform.position;
                RacingController.instance.transform.forward = checkPoint.transform.forward;
                RacingController.instance.rb.velocity = Vector3.zero;
            }
        }
    }
}
