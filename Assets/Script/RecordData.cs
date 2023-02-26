using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordData
{
    public Dictionary<string, string> attackerData { get; private set; }
    public Dictionary<string, string> victimData { get; private set; }
    public int isHanging { get; private set; }

    public RecordData()
    {
        attackerData = TableManager.GetData(null, null);
        victimData = TableManager.GetData(attackerData["familyName"], attackerData["name"]);


        //사형 판별//
        List<List<Dictionary<string, List<string>>>> judgeList = TableManager.judgeT;
        int day = HangingManager.day;
        bool f = false;

        Debug.Log("Grade : " + attackerData["positionGrade"]);
        Debug.Log("CrimeGrade : " + attackerData["crimeGrade"]);
        Debug.Log("CrimeReason : " + attackerData["crimeReason"]);
        Debug.Log("AttackerMove : " + attackerData["move"]);
        Debug.Log("VictimMove : " + victimData["move"]);

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
                    Dictionary<string, string> compareList;

                    compareList = header.Length > 6 && header.Substring(0, 6).Equals("victim") ? victimData : attackerData;

                    foreach (string str in judgeList[i][j][header])
                    {
                        if (str.Equals(compareList[header]))
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