using System;
using UnityEngine;

public class WorkSystemData  : MonoBehaviour
{
    public bool isClone;        //복제인간 여부
    public bool isCut;          //자상 여부
    public bool isBurn;         //화상 여부
    public bool isRebelForces;  //반란군 여부

    public DateTime currentDate;        //일자 설정 위한 임시Date값(추후 수정 필요)
    public DateTime markerOccurDate;    //승급 표식 발생일
    public DateTime promotionDate;      //승급일
    public DateTime burnOccurDate;      //화상자국 발생일
    public DateTime birthday;           //태어난 일자

    public string reasonForChange;  //시민등급 표식 변경 사유

    private void Awake()
    {
        //bool값 확률(?)과 일자 설정은 기획 수정 후 변경 필요
        SetRandomBool(isClone);
        SetRandomBool(isCut);
        SetRandomBool(isBurn);
        SetRandomBool(isRebelForces);

        currentDate = System.DateTime.Now;
        burnOccurDate = GetRandomDate(currentDate, currentDate.AddYears(1));    //일자는 임시값
        birthday = GetRandomDate(currentDate, currentDate.AddYears(1));         //일자는 임시값
        promotionDate = GetRandomDate(currentDate, currentDate.AddYears(1));
        markerOccurDate = GetRandomDate(currentDate, currentDate.AddYears(1));

        reasonForChange = GetRandomReasonForChange();
    }

    private void SetRandomBool(bool boolData)
    {
        System.Random random = new System.Random();

        if(random.Next(0,3) == 1)
        {
            boolData = true;
        }
        else
        {
            boolData = false;
        }
    }

    DateTime GetRandomDate(DateTime startDate, DateTime endDate)
    {
        TimeSpan timeSpan = endDate - startDate;
        TimeSpan randomTimeSpan = new TimeSpan((long)(UnityEngine.Random.value * timeSpan.Ticks));
        DateTime randomDate = startDate + randomTimeSpan;

        return randomDate;
    }

    String GetRandomReasonForChange()
    {
        string reason = "";
        string[] reasons = { "모범", "국가 공헌", "국정 지원", "해당 없음" };

        reason = reasons[UnityEngine.Random.Range(0, reasons.Length)];

        return reason;
    }
}

