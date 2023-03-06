using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordData
{
    public Dictionary<string, string> attackerData { get; private set; }
    public Dictionary<string, string> victimData { get; private set; }
    public int isHanging;

    public RecordData(TableManager tableManager)
    {
        attackerData = tableManager.GetData(null, null);
        victimData = tableManager.GetData(attackerData["familyName"], attackerData["name"]);

        Debug.Log("PositionGrade : " + attackerData["positionGrade"]);
        Debug.Log("FamilyGrade : " + attackerData["familyGrade"]);
        Debug.Log("CrimeGrade : " + attackerData["crimeGrade"]);
        Debug.Log("CrimeReason : " + attackerData["crimeReason"]);
        Debug.Log("AttackerMove : " + attackerData["move"]);
        Debug.Log("VictimMove : " + victimData["move"]);
        Debug.Log("crimeRecord : " + attackerData["crimeRecord"]);

        Judgement();
    }

    //사형 판별//
    void Judgement()
    {
        List<List<Dictionary<string, List<string>>>> judgeList = TableManager.judgeT;
        List<int> dayList = GetJudgementDay(HangingManager.day);
        isHanging = 1;

        for (int i= 0; i < dayList.Count; i++)
        {
            int day = dayList[i];
            for (int j = 0; j < judgeList[day].Count; j++)
            {
                List<string> headerList = judgeList[day][0]["header"];
                bool isMatch = true;
                for (int k = 0; k < headerList.Count - 1; k++)
                {
                    string header = headerList[k], subHeader;
                    bool subMatch = false;
                    Dictionary<string, string> compareList;

                    if (header.Equals("ask")) continue;

                    //attacker, victim 명시된 헤더 분리//
                    if (header.Length > 6 && header.Substring(0, 6).Equals("victim"))
                    {
                        compareList = victimData;
                        subHeader = header.Substring(6);
                    }
                    else if (header.Length > 8 && header.Substring(0, 8).Equals("attacker"))
                    {
                        compareList = attackerData;
                        subHeader = header.Substring(8);
                    }
                    else
                    {
                        compareList = attackerData;
                        subHeader = header;
                    }

                    //하나의 셀의 여러 개의 값 확인//
                    foreach (string str in judgeList[day][j][header])
                    {
                        if (str.Equals(compareList[subHeader]))
                        {
                            subMatch = true;
                            break;
                        }
                    }
                    if (!subMatch)
                    {
                        isMatch = false;
                        break;
                    }
                }

                //검색 완료//
                if (isMatch)
                {
                    isHanging = int.Parse(judgeList[day][j]["judgement"][0]);
                    if (judgeList[day][j].ContainsKey("ask"))
                    {
                        attackerData["ask"] = judgeList[day][j]["ask"][0];
                        Debug.Log(attackerData["ask"]);
                    }
                    Debug.Log(day + ",," + j);
                    Debug.Log(isHanging);

                    if(isHanging == 2)
                    {
                        i--;
                        attackerData["crimeGrade"] = (int.Parse(attackerData["crimeGrade"]) - 1).ToString();
                        break;
                    }
                    else return;
                }
            }
        }
    }

    List<int> GetJudgementDay(int day)
    {
        List<int> list = new List<int>();

        switch (day)
        {
            case 1:
            case 4:
                list.Add(day - 1);
                break;
            case 2:
            case 3:
                list.Add(day - 1);
                list.Add(0);
                break;
            default:
                break;
        }

        return list;
    }
}