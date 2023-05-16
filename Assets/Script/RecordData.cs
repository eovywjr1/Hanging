using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordData
{
    public Dictionary<string, string> attackerData { get; private set; }
    //public Dictionary<string, string> attackerData { get; set; }    //임시
    public Dictionary<string, string> victimData { get; private set; }
    /*public Dictionary<string, string> victimData { get; set; }  //임시 */

    public Dictionary<string, List<string>> lieORInfoError;
    public int isHanging;
    public string[] correctState { get; private set; }
    public string[] currentState { get; private set; }
    public int lieORinfoErrorValue; //0-> 아무것도 아님, 1->lie, 2->infoError

     public RecordData(TableManager tableManager, ReadPrisonerInfo readPrisonerInfo)
    {
        lieORInfoError = new Dictionary<string, List<string>>();

        attackerData = tableManager.GetData(null, null, ref lieORInfoError);    //유민) 이 부분이 prisonerInfo 넣는 부분
        victimData = tableManager.GetData(attackerData["familyName"], attackerData["name"], ref lieORInfoError);

        Debug.Log("PositionGrade : " + attackerData["positionGrade"]);
        Debug.Log("FamilyGrade : " + attackerData["familyGrade"]);
        Debug.Log("CrimeGrade : " + attackerData["crimeGrade"]);
        Debug.Log("CrimeReason : " + attackerData["crimeReason"]);
        Debug.Log("AttackerJob : " + attackerData["job"]);
        Debug.Log("이거확인!! AttackerMove : " + attackerData["move"]);
        Debug.Log("이거확인!!VictimMove : " + victimData["move"]);
        //Debug.Log("CrimeRecord : " + attackerData["crimeRecord"]);
        //Debug.Log("Lie : " + attackerData["lie"]);
        //Debug.Log("InfoError : " + attackerData["infoError"]);

        Judgement(readPrisonerInfo);
        lieORinfoErrorValue = 0; // 0으로 초기화
        MakeStatement(tableManager);  
    }
    //사형 판별//
    void Judgement(ReadPrisonerInfo readPrisonerInfo)
    {
        List<List<Dictionary<string, List<string>>>> judgeList = TableManager.judgeT;

        isHanging = readPrisonerInfo.getAnswer();

        Debug.Log("사형판결 " + isHanging);

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
                        if(readPrisonerInfo.GetAsk() != null)
                        {
                            attackerData["ask"] = readPrisonerInfo.GetAsk();
                        }
                        else
                        {
                            attackerData["ask"] = judgeList[i][j]["ask"][0];
                        }
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
    public Dictionary<string, List<string>> GetLieORInfoError()
    {
        return lieORInfoError;
    }

    //진술서 데이터 생성
    public void MakeStatement(TableManager tableManager)
    {
        correctState = new string[4]{ attackerData["familyName"] +" "+ attackerData["name"] , attackerData["crime"], attackerData["crimePlaceText"], attackerData["crimeReasonText"] };
        currentState = new string[4]{ attackerData["familyName"] +" "+ attackerData["name"] , attackerData["crime"], attackerData["crimePlaceText"], attackerData["crimeReasonText"] };

        for (int i = 0; i < 4; i++)
        {
            Debug.Log("맞는거"+correctState[i]);
        }

        Debug.Log("실행생성");
        if (lieORInfoError.ContainsKey("lie") || lieORInfoError.ContainsKey("infoError"))
        {
            if (lieORInfoError.ContainsKey("lie")){
                Debug.Log("위증!");
                lieORinfoErrorValue = 1;
                

                for(int i = 0; i < lieORInfoError["lie"].Count; i++)
                {
                    if (lieORInfoError["lie"][i] == "name") //이름이 위증인 경우 위조된 정보로 변경
                    {
                        currentState[0] = tableManager.GetRandomStatement("nameT");
                    }
                    if (lieORInfoError["lie"][i] == "familyName") //성이 위증인 경우 ~
                    {
                        currentState[1] = tableManager.GetRandomStatement("familyName");
                    }
                    if (lieORInfoError["lie"][i] == "crime") //성이 위증인 경우
                    {
                        currentState[2] = tableManager.GetRandomStatement("crime");
                    }
                    if (lieORInfoError["lie"][i] == "crimePlaceText") //성이 위증인 경우
                    {
                        currentState[3] = tableManager.GetRandomStatement("crimePlaceText");
                    }
                    if (lieORInfoError["lie"][i] == "crimeReasonText") //성이 위증인 경우
                    {
                        currentState[4] = tableManager.GetRandomStatement("crimeReasonText"); ;
                    }
                }
            }
            else
            {
                lieORinfoErrorValue = 2;
                Debug.Log("정보오류!");

                for (int i = 0; i < lieORInfoError["infoError"].Count; i++)
                {
                    if (lieORInfoError["infoError"][i] == "name") //이름이 위증인 경우 위조된 정보로 변경
                    {
                        currentState[0] = tableManager.GetRandomStatement("nameT");
                    }
                    if (lieORInfoError["infoError"][i] == "familyName") //성이 위증인 경우 ~
                    {
                        currentState[1] = tableManager.GetRandomStatement("familyName");
                    }
                    if (lieORInfoError["infoError"][i] == "crime") //성이 위증인 경우
                    {
                        currentState[2] = tableManager.GetRandomStatement("crime");
                    }
                    if (lieORInfoError["infoError"][i] == "crimePlaceText") //성이 위증인 경우
                    {
                        currentState[3] = tableManager.GetRandomStatement("crimePlaceText");
                    }
                    if (lieORInfoError["infoError"][i] == "crimeReasonText") //성이 위증인 경우
                    {
                        currentState[4] = tableManager.GetRandomStatement("crimeReasonText"); ;
                    }
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            Debug.Log("진술서에 보여지고 있는 것 : " + currentState[i]);
        }

    }
}