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
        Debug.Log("AttackerJob : " + attackerData["job"]);
        Debug.Log("AttackerMove : " + attackerData["move"]);
        Debug.Log("VictimMove : " + victimData["move"]);
        Debug.Log("CrimeRecord : " + attackerData["crimeRecord"]);
        Debug.Log("Lie : " + attackerData["lie"]);

        Judgement();
    }

    //사형 판별//
    void Judgement()
    {
        List<List<Dictionary<string, List<string>>>> judgeList = TableManager.judgeT;
        
        isHanging = 1;

        for (int i= HangingManager.day - 1; i >= 0; i--)
        {
            for (int j = 0; j < judgeList[i].Count; j++)
            {
                List<string> headerList = judgeList[i][0]["header"];
                bool isMatch = true;
                for (int k = 0; k < headerList.Count - 1; k++)
                {
                    string header = headerList[k], subHeader;
                    bool subMatch = false;
                    Dictionary<string, string> compareList;

                    //ask는 이 함수에서 판단에 직접적인 영향 x//
                    if (header.Equals("ask")) continue;

                    //attacker, victim 명시된 헤더 분리해서 각 data에 접근//
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
                    foreach (string str in judgeList[i][j][header])
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
                    isHanging = int.Parse(judgeList[i][j]["judgement"][0]);
                    if (judgeList[i][j].ContainsKey("ask"))
                    {
                        attackerData["ask"] = judgeList[i][j]["ask"][0];
                        Debug.Log(attackerData["ask"]);
                    }
                    Debug.Log(i + ",," + j);
                    Debug.Log(isHanging);


                    //범죄등급 +1 증가 후 다시 판단//
                    if (isHanging == 2)
                    {
                        //isHanging 값이 계속 2면 범죄등급이 계속 +1 될듯
                        //한번만 +1 된 범죄등급이 된 범죄자의 judgement를 0 또는 1으로 바꿔주는 구현 필요
                        i++;
                        attackerData["crimeGrade"] = (int.Parse(attackerData["crimeGrade"]) - 1).ToString();
                        break;
                    }
                    else return;
                }
            }
        }
    }
}