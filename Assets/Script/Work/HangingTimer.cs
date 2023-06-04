using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class HangingTimer : MonoBehaviour
{
    bool isTimer;
    [SerializeField] private float workTime = 0f;
    const float workMaxTime = 210f;
    Slider timerSlider;

    private void Awake()
    {
        workTime = workMaxTime;
        timerSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (isTimer)
        {
            workTime -= Time.deltaTime;
            timerSlider.value = workTime / workMaxTime;

            if (workTime <= 0)
            {
                isTimer = false;
                workTime = workMaxTime;

                StartCoroutine(FindObjectOfType<HangingManager>().endDay());
            }
        }
    }

    public void SetTimer(bool _isTimer)
    {
        isTimer = _isTimer;
    }
}