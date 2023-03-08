using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WindowShowData : MonoBehaviour
{
    WindowSetSize windowSetSize;
    RecordData recordData;

    void Awake()
    {
        windowSetSize = FindObjectOfType<WindowSetSize>();
    }

    public void SetText()
    {
        recordData = FindObjectOfType<AttackerInfo>().recordData;

        //텍스트 설정//
        string text = "가해자: " + recordData.attackerData["familyName"] + " " + recordData.attackerData["name"] + "\n"
            + "죄목: " + recordData.attackerData["crime"] + "\n"
            + "발생 장소: " + recordData.attackerData["crimePlaceText"] + " 등급" + "\n"
            + "피해자: " + recordData.victimData["familyName"] + " " + recordData.victimData["name"] + "\n"
            + "경위: " + recordData.attackerData["crimeReasonText"];

        GetComponent<TextMeshProUGUI>().text = text.Replace("\\n", "\n");

        //창 크기 조절//
        int maxlength = Mathf.Max((recordData.attackerData["familyName"] + recordData.attackerData["name"]).Length, 
            (recordData.victimData["familyName"] + recordData.victimData["name"]).Length);
        float width = 2.3f + 0.15f * (maxlength - 3);
        windowSetSize.SetSize(width);
    }
}
