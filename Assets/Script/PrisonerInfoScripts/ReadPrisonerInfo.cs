using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// 1~7일차 사형수 정보 데이터 관리

public class ReadPrisonerInfo : MonoBehaviour
{
    public int order;   //사형수 번호(순서) 관리하는 스크립트에서 order 가져와 사용하도록 변경 필요
    string fileName;
    int day = HangingManager.day;
    //public int tmpDay;

    List<Dictionary<string, object>> data;
    public Dictionary<string, string> currentPrisonerInfo = new Dictionary<string, string>();   //사형수 정보에 대한 딕셔너리

    /*
        1. currentPrisonerInfo 가져가기 전 각 사형수에 대한 order 배정 필요.
        2. FieldNameOfCSV에서 fieldNames 가져와
            currentPrisonerInfo[fieldNameOfCSV.fieldNames[i]] 처럼 사용해 각 필드의 데이터 가져오기 가능
            (또는 직접 ex) currentPrisonerInfo["Grade"] 처럼 사용
    */

    public FieldNameOfCSV fieldNameOfCSV;
    TableManager tableManager;
    HangingManager hangingManager;

    private void Awake()
    {
        char charValue = (char)(day + '0');
        fileName = "Prisoner_day" + charValue;

        FileInfo fileInfo = new FileInfo(fileName);
        if (fileInfo.Exists == false)
        {
            Debug.Assert(false, "조민수 comment : " + fileName + "이 없습니다 파일을 추가해주세요.");
            return;
        }

        data = CSVReader2.Read(fileName);

        fieldNameOfCSV = GetComponent<FieldNameOfCSV>();
        tableManager = FindObjectOfType<TableManager>();
        hangingManager = FindObjectOfType<HangingManager>();

        order = hangingManager.judgeCount;
    }

    private void Start()
    {
        if (data == null)
            return;

        for (int i = 0; i < data.Count; i++)
        {
            for (int j = 1; j < fieldNameOfCSV.fieldNames.Count; j++)
            {
                string fieldName = fieldNameOfCSV.fieldNames[j];
                string curData = data[i][fieldName].ToString();

                if (curData.Length >= 2 && int.TryParse(curData, out int output))
                {
                    SetRandomValue(i, fieldName);
                }
            }
        }

        setCurrentPrisonerInfo(order);
    }

    void SetRandomValue(int order, string fieldName)
    {
        string tmpString = data[order][fieldName].ToString();
        int randomIdx = UnityEngine.Random.Range(0, tmpString.Length);
        int randomValue = int.Parse(tmpString[randomIdx].ToString());

        data[order][fieldName] = (char)(randomValue + '0');
    }

    void setCurrentPrisonerInfo(int currentOrder)
    {
        for (int i = 1; i < fieldNameOfCSV.fieldNames.Count; i++)
        {
            string fieldName = fieldNameOfCSV.fieldNames[i];
            currentPrisonerInfo.Add(fieldName, data[currentOrder][fieldName].ToString()) ;
        }
    }

    public int GetGrade()
    {
        if (currentPrisonerInfo.ContainsKey("Grade"))
        {
            return int.Parse(currentPrisonerInfo["Grade"]);
        }
        else return -1;
    }

    public int GetCrimeGrade()
    {
        if (currentPrisonerInfo.ContainsKey("CrimeGrade"))
        {
            return int.Parse(currentPrisonerInfo["CrimeGrade"]);
        }
        else return -1;
    }

    public int GetCrimeReason()
    {
        if(currentPrisonerInfo.ContainsKey("CrimeReason"))
        {
            return int.Parse(currentPrisonerInfo["CrimeReason"]);
        }
        return -1;
    }

    public int GetAttackerMove()
    {
        if (currentPrisonerInfo.ContainsKey("AttackerMove"))
        {
            return int.Parse(currentPrisonerInfo["AttackerMove"]);
        }
        return -1;
    }

    public int GetVictimMove()
    {
        if (currentPrisonerInfo.ContainsKey("VictimMove"))
        {
            return int.Parse(currentPrisonerInfo["VictimMove"]);
        }
        return -1;
    }

    public int GetAttackerJob()
    {
        if (currentPrisonerInfo.ContainsKey("AttackerJob"))
        {
            return int.Parse(currentPrisonerInfo["AttackerJob"]);
        }
        return -1;
    }

    public int GetVictimJob()
    {
        if (currentPrisonerInfo.ContainsKey("VictimJob"))
        {
            return int.Parse(currentPrisonerInfo["VictimJob"]);
        }
        return -1;
    }

    public string GetVictimGrade()
    {
        if (currentPrisonerInfo.ContainsKey("VictimGrade"))
        {
            return currentPrisonerInfo["VictimGrade"];
        }
        return null;
    }

    public int GetCrimeRecord()
    {
        if (currentPrisonerInfo.ContainsKey("CrimeRecord"))
        {
            return int.Parse(currentPrisonerInfo["CrimeRecord"]);
        }
        else return -1;
    }

    public int GetLie()
    {
        if (currentPrisonerInfo.ContainsKey("Lie"))
        {
            return int.Parse(currentPrisonerInfo["Lie"]);
        }
        return -1;
    }

    public int GetInfoError()
    {
        if (currentPrisonerInfo.ContainsKey("InfoError"))
        {
            return int.Parse(currentPrisonerInfo["InfoError"]);
        }
        return -1;
    }

    public string GetAsk()
    {
        if (currentPrisonerInfo.ContainsKey("Ask"))
        {
            return currentPrisonerInfo["Ask"];
        }
        else return null;
    }

    public int getAnswer()
    {
        int answer = 1;
        if (currentPrisonerInfo.ContainsKey("Answer"))
        {
            answer = int.Parse(currentPrisonerInfo["Answer"]);
        }
        return answer;
    }
}
