using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordData
{
    public string name { get; private set; }
    public string fname { get; private set; }
    public string victimName { get; private set; }
    public string victimFamilyName { get; private set; }
    public string crime { get; private set; }
    public string crimeReasonText { get; private set; }
    public string crimePlaceText { get; private set; }
    public int isHanging { get; private set; }

    public RecordData()
    {
        Dictionary<string, string> data = TableManager.GetData();

        name = data["name"];
        fname = data["fname"];
        victimName = data["victimName"];
        victimFamilyName = data["victimFamilyName"];
        crime = data["crime"];
        crimeReasonText = data["crimeReasonText"];
        crimePlaceText = data["crimePlaceText"];

        //사형 판별//
        List<List<Dictionary<string, List<string>>>> judgeList = TableManager.judgeT;
        int day = HangingManager.day;
        bool f = false;

        Debug.Log("Grade : " + data["positionGrade"]);
        Debug.Log("CrimeGrade : " + data["crimeGrade"]);
        Debug.Log("CrimeReason : " + data["crimeReason"]);
        Debug.Log("AttackerMove : " + data["attackerMove"]);
        Debug.Log("VictimMove : " + data["victimMove"]);

        for (int i = day - 1; i >= 0; i--)
        {
            if (f) break;
            for (int j = 0; j < judgeList[i].Count; j++)
            {
                List<string> headerList = judgeList[i][0]["header"];
                bool isMatch = true;
                for (int k = 0; k < headerList.Count - 1; k++)
                {
                    string header = headerList[k];
                    bool subMatch = false;
                    foreach (string str in judgeList[i][j][header])
                    {
                        if (str.Equals(data[header]))
                        {
                            subMatch = true;
                            break;
                        }
                    }

                    if (!subMatch) isMatch = false;
                }

                if (isMatch)
                {
                    f = true;
                    isHanging = int.Parse(judgeList[i][j]["judgement"][0]);
                    Debug.Log(j);
                    Debug.Log(isHanging);
                    break;
                }
            }
        }
    }
}