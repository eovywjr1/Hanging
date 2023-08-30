using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeartRate : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    TortureManage tortureManage;

    public enum HeartRateState
    {
        //True일 땐 고문해도 답변 변하지X, 나머지 경우는 답변 변할 수 있음
        True,
        TrueOrFalse,
        False
    }

    int heartRate;
    HeartRateState currentState;


    public HeartRateState DetermineHeartRateState(int heartRate)
    {
        if(heartRate >= 60 && heartRate <= 79)
        {
            return HeartRateState.True;
        }
        else if(heartRate >= 80 && heartRate <= 99)
        {
            return HeartRateState.TrueOrFalse;
        }
        else if(heartRate >= 100 && heartRate <= 120)
        {
            return HeartRateState.False;
        }
        else
        {
            return HeartRateState.False;
        }
    }

    private void Awake()
    {
        tortureManage = FindObjectOfType<TortureManage>();
    }

    void Start()
    {
        SetHeartRate();
    }

    private void Update()
    {
        if(tortureManage.tortureStatus)
        {
            tortureManage.tortureStatus = false;

            //심박수 80~99 또는 100~120일 경우 사형수의 답변 바뀜 (미작업)
            if (currentState == HeartRateState.TrueOrFalse || currentState == HeartRateState.False)
            {
                SetHeartRate(); //심박수 바뀜
            }
        }
    }

    void SetHeartRate()
    {
        // | 60~79:무조건 진실 || 80~99:진실or거짓 || 100~120:거짓 |
        heartRate = Random.Range(60, 121);

        currentState = DetermineHeartRateState(heartRate);

        text.text = heartRate.ToString() + "\n(심박수)";
    }

}
