using System;
using UnityEngine;


public class BodySearchData : MonoBehaviour
{
    public DateTime currentDate;
    public DateTime randomDate1;    //등급 표식 생성일
    public DateTime randomDate2;    //승급일
    public String reasonForChange;  //시민 등급 표식 변경 사유

    private void Awake()
    {
        currentDate = System.DateTime.Now;
        System.DateTime randomDate1 = GetRandomDate(currentDate, currentDate.AddYears(1));
        System.DateTime randomDate2 = GetRandomDate(currentDate, currentDate.AddYears(1));
        reasonForChange = GetRandomReasonForChange();
    }

    private DateTime GetRandomDate(DateTime startDate, DateTime endDate)
    {
        TimeSpan timeSpan = endDate - startDate;
        TimeSpan randomTimeSpan = new TimeSpan((long)(UnityEngine.Random.value * timeSpan.Ticks));
        DateTime randomDate = startDate + randomTimeSpan;

        return randomDate;
    }

    private String GetRandomReasonForChange()
    {
        string reason = "";
        string[] reasons = { "모범", "국가 공헌", "국정 지원", "해당 없음" };

        reason = reasons[UnityEngine.Random.Range(0, reasons.Length)];

        return reason;
    }
}

