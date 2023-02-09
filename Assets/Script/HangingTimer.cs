using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HangingTimer : MonoBehaviour
{
    bool isTimer = true;
    [SerializeField] private float workTime = 0f, workMaxTime = 10f;
    Slider timerSlider;
    HangingManager hangingManager;

    private void Awake()
    {
        workTime = workMaxTime;
        timerSlider = GetComponent<Slider>(); 
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
                hangingManager.attackerMouseMove.SetisPossibleTodesstrafe(false);
                //hangingManager.ConvertScene();
                Debug.Log("Game Over");
            }
        }
    }
}