using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordData
{
    public Dictionary<string, string> attackerData { get; private set; }
    public Dictionary<string, string> victimData { get; private set; }
    public Dictionary<string, List<string>> lieORInfoError;
    public int isHanging;
    public string[] correctState { get; private set; }
    public string[] currentState { get; private set; }
    public int lieORinfoErrorValue; //0-> �ƹ��͵� �ƴ�, 1->lie, 2->infoError

    public RecordData(TableManager tableManager)
    {
        lieORInfoError = new Dictionary<string, List<string>>();

        attackerData = tableManager.GetData(null, null, ref lieORInfoError);
        victimData = tableManager.GetData(attackerData["familyName"], attackerData["name"], ref lieORInfoError);
        
        //Debug.Log("PositionGrade : " + attackerData["positionGrade"]);
        //Debug.Log("FamilyGrade : " + attackerData["familyGrade"]);
        //Debug.Log("CrimeGrade : " + attackerData["crimeGrade"]);
        //Debug.Log("CrimeReason : " + attackerData["crimeReason"]);
        //Debug.Log("AttackerJob : " + attackerData["job"]);
        //Debug.Log("AttackerMove : " + attackerData["move"]);
        //Debug.Log("VictimMove : " + victimData["move"]);
        //Debug.Log("CrimeRecord : " + attackerData["crimeRecord"]);
        //Debug.Log("Lie : " + attackerData["lie"]);
        //Debug.Log("InfoError : " + attackerData["infoError"]);

        Judgement();
        lieORinfoErrorValue = 0; // 0���� �ʱ�ȭ
        MakeStatement(tableManager);  
    }

    //���� �Ǻ�//
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

                    //ask�� �� �Լ����� �Ǵܿ� �������� ���� x//
                    if (header.Equals("ask")) continue;

                    //attacker, victim ��õ� ��� �и��ؼ� �� data�� ����//
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

                    //�ϳ��� ���� ���� ���� �� Ȯ��//
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

                //�˻� �Ϸ�//
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


                    //���˵�� +1 ���� �� �ٽ� �Ǵ�//
                    if (isHanging == 2)
                    {
                        //isHanging ���� ��� 2�� ���˵���� ��� +1 �ɵ�
                        //�ѹ��� +1 �� ���˵���� �� �������� judgement�� 0 �Ǵ� 1���� �ٲ��ִ� ���� �ʿ�
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

    //������ ������ ����
    public void MakeStatement(TableManager tableManager)
    {
        correctState = new string[4]{ attackerData["familyName"] +" "+ attackerData["name"] , attackerData["crime"], attackerData["crimePlaceText"], attackerData["crimeReasonText"] };
        currentState = new string[4]{ attackerData["familyName"] +" "+ attackerData["name"] , attackerData["crime"], attackerData["crimePlaceText"], attackerData["crimeReasonText"] };

        for (int i = 0; i < 4; i++)
        {
            Debug.Log("�´°�"+correctState[i]);
        }

        Debug.Log("�������");
        if (lieORInfoError.ContainsKey("lie") || lieORInfoError.ContainsKey("infoError"))
        {
            if (lieORInfoError.ContainsKey("lie")){
                Debug.Log("����!");
                lieORinfoErrorValue = 1;
                

                for(int i = 0; i < lieORInfoError["lie"].Count; i++)
                {
                    if (lieORInfoError["lie"][i] == "name") //�̸��� ������ ��� ������ ������ ����
                    {
                        currentState[0] = tableManager.GetRandomStatement("nameT");
                    }
                    if (lieORInfoError["lie"][i] == "familyName") //���� ������ ��� ~
                    {
                        currentState[1] = tableManager.GetRandomStatement("familyName");
                    }
                    if (lieORInfoError["lie"][i] == "crime") //���� ������ ���
                    {
                        currentState[2] = tableManager.GetRandomStatement("crime");
                    }
                    if (lieORInfoError["lie"][i] == "crimePlaceText") //���� ������ ���
                    {
                        currentState[3] = tableManager.GetRandomStatement("crimePlaceText");
                    }
                    if (lieORInfoError["lie"][i] == "crimeReasonText") //���� ������ ���
                    {
                        currentState[4] = tableManager.GetRandomStatement("crimeReasonText"); ;
                    }
                }
            }
            else
            {
                lieORinfoErrorValue = 2;
                Debug.Log("��������!");

                for (int i = 0; i < lieORInfoError["infoError"].Count; i++)
                {
                    if (lieORInfoError["infoError"][i] == "name") //�̸��� ������ ��� ������ ������ ����
                    {
                        currentState[0] = tableManager.GetRandomStatement("nameT");
                    }
                    if (lieORInfoError["infoError"][i] == "familyName") //���� ������ ��� ~
                    {
                        currentState[1] = tableManager.GetRandomStatement("familyName");
                    }
                    if (lieORInfoError["infoError"][i] == "crime") //���� ������ ���
                    {
                        currentState[2] = tableManager.GetRandomStatement("crime");
                    }
                    if (lieORInfoError["infoError"][i] == "crimePlaceText") //���� ������ ���
                    {
                        currentState[3] = tableManager.GetRandomStatement("crimePlaceText");
                    }
                    if (lieORInfoError["infoError"][i] == "crimeReasonText") //���� ������ ���
                    {
                        currentState[4] = tableManager.GetRandomStatement("crimeReasonText"); ;
                    }
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            Debug.Log("�������� �������� �ִ� �� : " + currentState[i]);
        }

    }
}