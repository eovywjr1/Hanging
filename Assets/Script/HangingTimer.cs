using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HangingTimer : MonoBehaviour
{
    private bool isTimer = true;
    private float workTime, workMaxTime = 10;

    [SerializeField] private Slider timerSlider;
    private HangingManager hangingManager;

    private void Start()
    {
        workTime = workMaxTime;
        hangingManager = FindObjectOfType<HangingManager>();
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
                hangingManager.hangingMove.SetisPossibleTodesstrafe(false);
                hangingManager.ConvertScene();
            }
        }
    }
}