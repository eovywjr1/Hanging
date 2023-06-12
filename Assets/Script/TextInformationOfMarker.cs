using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextInformationOfMarker : MonoBehaviour
{
    [SerializeField] TMP_Text information;

    private void Start()
    {
        System.DateTime currentDate = System.DateTime.Now;
        System.DateTime randomDate1 = GetRandomDate(currentDate, currentDate.AddYears(1));
        System.DateTime randomDate2 = GetRandomDate(currentDate, currentDate.AddYears(1));

        information.text = "등급 표식 생성일 : " + randomDate1.ToString("yyyy-MM-dd") + "\n";
        information.text += "승급일 : " + randomDate2.ToString("yyyy-MM-dd");
    }

    private System.DateTime GetRandomDate(System.DateTime startDate, System.DateTime endDate)
    {
        System.TimeSpan timeSpan = endDate - startDate;
        System.TimeSpan randomTimeSpan = new System.TimeSpan((long)(Random.value * timeSpan.Ticks));
        System.DateTime randomDate = startDate + randomTimeSpan;

        return randomDate;
    }
}
