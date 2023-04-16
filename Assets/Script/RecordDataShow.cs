using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordDataShow : WindowShowData
{
    private void Start()
    {
        Line.lineList[0].GetShowData(ShowData);
    }

    void ShowData()
    {
        RecordData recordData = FindObjectOfType<AttackerInfo>().recordData;

        string str = "������: " + recordData.attackerData["familyName"] + " " + recordData.attackerData["name"] + "\n"
            + "�˸�: " + recordData.attackerData["crime"] + "\n"
            + "�߻� ���: " + recordData.attackerData["crimePlaceText"] + " ���\n"
            + "������: " + recordData.victimData["familyName"] + " " + recordData.victimData["name"] + "\n"
            + "����: " + recordData.attackerData["crimeReasonText"];

        int maxLength = Mathf.Max((recordData.attackerData["familyName"] + recordData.attackerData["name"]).Length,
                        (recordData.victimData["familyName"] + recordData.victimData["name"]).Length);
        float width = 2.3f + 0.22f * (maxLength - 3);

        SetText(str);
        SetTextSize(width);
    }
}
