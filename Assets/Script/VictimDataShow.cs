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

        int maxLength = Mathf.Max(recordData.victimData["familyName"].Length + recordData.victimData["name"].Length,
                        2 + recordData.victimData["jobText"].Length);
        float width = 1.55f + 0.22f * (maxLength - 4);

        SetText(str);
        if (maxLength > 4) SetTextSize(width);
    }
}
