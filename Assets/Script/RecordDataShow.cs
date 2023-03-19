using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordDataShow : WindowShowData
{
    private void Start()
    {
        Line.lineList[Line.lineList.Count - 1].GetShowData(ShowData);
    }

    void ShowData()
    {
        recordData = FindObjectOfType<AttackerInfo>().recordData;

        string str = "가해자: " + recordData.attackerData["familyName"] + " " + recordData.attackerData["name"] + "\n"
            + "죄목: " + recordData.attackerData["crime"] + "\n"
            + "발생 장소: " + recordData.attackerData["crimePlaceText"] + " 등급\n"
            + "피해자: " + recordData.victimData["familyName"] + " " + recordData.victimData["name"] + "\n"
            + "경위: " + recordData.attackerData["crimeReasonText"];

        int maxLength = Mathf.Max((recordData.attackerData["familyName"] + recordData.attackerData["name"]).Length,
                        (recordData.victimData["familyName"] + recordData.victimData["name"]).Length);
        float width = 2.3f + 0.22f * (maxLength - 3);

        SetText(str);
        SetTextSize(width);
    }
}
