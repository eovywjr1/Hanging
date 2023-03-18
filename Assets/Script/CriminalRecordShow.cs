using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriminalRecordShow : WindowShowData
{
    private void Start()
    {
        ShowData();
    }

    void ShowData()
    {
        recordData = FindObjectOfType<AttackerInfo>().recordData;

        string str = recordData.attackerData["familyName"] + " " + recordData.attackerData["name"] + "\n"
            + "Àü°ú: " + recordData.attackerData["crimeRecordText"];

        int maxLength = recordData.attackerData["familyName"].Length + recordData.attackerData["name"].Length;
        float width = 1.55f + 0.22f * (maxLength - 4);
        
        SetText(str);
        if (maxLength > 4) SetTextSize(width);
    }
}
