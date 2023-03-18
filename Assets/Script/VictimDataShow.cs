using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictimDataShow : WindowShowData
{
    private void Start()
    {
        ShowData();
    }

    void ShowData()
    {
        recordData = FindObjectOfType<AttackerInfo>().recordData;

        string str = recordData.victimData["familyName"] + " " + recordData.victimData["name"] + "\n"
            + "성별: " + recordData.victimData["gender"] + "\n"
            + "나이: " + recordData.victimData["age"] + "세\n"
            + "직업: " + recordData.victimData["jobText"];

        int maxLength = (recordData.victimData["familyName"] + recordData.victimData["name"]).Length;
        float width = 1.8f + 0.2f * (maxLength - 7);

        SetText(str);
        if (maxLength > 7) SetTextSize(width);
    }
}
