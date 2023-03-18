using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerDataShow : WindowShowData
{
    private void Start()
    {
        Line.lineList[Line.lineList.Count - 1].GetShowData(ShowData);
    }

    void ShowData()
    {
        recordData = FindObjectOfType<AttackerInfo>().recordData;

        string str = recordData.attackerData["familyName"] + " " + recordData.attackerData["name"] + "\n"
            + "시민 등급: " + recordData.attackerData["positionGrade"] + "등급\n"
            + "성별: " + recordData.attackerData["gender"] + "\n"
            + "직업: " + recordData.attackerData["jobText"] + "\n"
            + "전과: 확인"; 

        int maxlength = Mathf.Max((recordData.attackerData["familyName"] + recordData.attackerData["name"]).Length,
                        (recordData.victimData["familyName"] + recordData.victimData["name"]).Length);

        SetText(str);
        SetTextSize(maxlength);
    }
}
