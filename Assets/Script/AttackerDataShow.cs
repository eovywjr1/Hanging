using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerDataShow : WindowShowData
{
    [SerializeField] GameObject exclamationMark;

    private void Start()
    {
        Line.lineList[Line.lineList.Count - 1].GetShowData(ShowData);
    }

    void ShowData()
    {
        recordData = FindObjectOfType<AttackerInfo>().recordData;

        string str = recordData.attackerData["familyName"] + " " + recordData.attackerData["name"] + "\n"
            + "성별: " + recordData.attackerData["gender"] + "\n"
            + "나이: " + recordData.attackerData["age"] + "세\n"
            + "직업: " + recordData.attackerData["jobText"] + "\n"
            + "전과: 확인";
        //전과 비활성 상태//
        if (HangingManager.day < 4)
        {
            str += "불가";
            exclamationMark.gameObject.SetActive(false);
        }

        int maxLength = Mathf.Max(recordData.attackerData["familyName"].Length + recordData.attackerData["name"].Length,
                        2 + recordData.attackerData["jobText"].Length);
        
        float width = 1.95f + 0.25f * (maxLength - 6);

        SetText(str);
        if (maxLength > 6) SetTextSize(width);
    }
}