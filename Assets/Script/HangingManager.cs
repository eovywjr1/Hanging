using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HangingManager : MonoBehaviour
{
    private bool isTimer = true;
    private float workTime, workMaxTime = 10;

    public Slider timerSlider;

    private void Start()
    {
        workTime = workMaxTime;
    }

    private void Update()
    {
        if (isTimer)
        {
            workTime -= Time.deltaTime;
            timerSlider.value = workTime / workMaxTime;
            if(workTime <= 0)
            {
                isTimer = false;
                workTime = workMaxTime;
            }
        }
    }
}