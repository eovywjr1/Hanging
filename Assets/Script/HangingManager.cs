using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class HangingManager : MonoBehaviour
{
    private bool isTimer = true;
    private float workTime, workMaxTime;

    private void Update()
    {
        if (isTimer)
        {
            workTime -= Time.deltaTime;
            if(workTime <= 0)
            {
                isTimer = false;
                workTime = workMaxTime;
                Debug.Log("time out");
            }
        }
    }
}