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
        string text = "가해자: " + recordData.fname + " " + recordData.name + "\n"
            + "죄목: " + recordData.crime + "\n"
            + "발생 장소: " + recordData.crimePlaceText + " 등급" + "\n"
            + "피해자: " + recordData.victimFamilyName + " " + recordData.victimName + "\n"
            + "경위: " + recordData.crimeReasonText;

        GetComponent<TextMeshProUGUI>().text = text.Replace("\\n", "\n");

        //창 크기 조절//
        int maxlength = Mathf.Max((recordData.fname + recordData.name).Length, (recordData.victimFamilyName + recordData.victimName).Length);
        float width = 2.3f + 0.3f * (maxlength - 4);
        windowSetSize.SetSize(width);
    }
}
