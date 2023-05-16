using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadPrisonerInfo : MonoBehaviour
{
    public int order;   //사형수 번호(순서) 관리하는 스크립트에서 order 가져와 사용하도록 변경 필요
    string fileName;
    int day = HangingManager.day;
    //public int tmpDay;

    List<Dictionary<string, object>> data;
    Dictionary<string, object> currentPrisonerInfo = new Dictionary<string, object>();   //사형수 정보에 대한 딕셔너리

    /*
        1. currentPrisonerInfo 가져가기 전 각 사형수에 대한 order 배정 필요.
        2. FieldNameOfCSV에서 fieldNames 가져와
            currentPrisonerInfo[fieldNameOfCSV.fieldNames[i]] 처럼 사용해 각 필드의 데이터 가져오기 가능
            (또는 직접 ex) currentPrisonerInfo["Grade"] 처럼 사용
    */

    public FieldNameOfCSV fieldNameOfCSV;
    TableManager tableManager;

    private void Awake()
    {
        char charValue = (char)(day + '0');

        fileName = "Prisoner_day";
        fileName += charValue;

        data = CSVReader2.Read(fileName);

        fieldNameOfCSV = GetComponent<FieldNameOfCSV>();
        tableManager = FindObjectOfType<TableManager>();
    }

    private void Start()
    {
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
/*        for (int i = 1; i < fieldNameOfCSV.fieldNames.Count; i++)
        {
            Debug.Log("currentPrisonerInfo.ContainsKey(" + fieldNameOfCSV.fieldNames[i] + "): " + currentPrisonerInfo.ContainsKey(fieldNameOfCSV.fieldNames[i]));
        }

        for (int i = 1; i < fieldNameOfCSV.fieldNames.Count; i++)
        {
            Debug.Log(fieldNameOfCSV.fieldNames[i] + "의 값은 " + currentPrisonerInfo[fieldNameOfCSV.fieldNames[i]]);
        }*/

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
            currentPrisonerInfo.Add(fieldName, data[currentOrder][fieldName]);
        }
    }

    public string GetGrade()
    {
        if (currentPrisonerInfo.ContainsKey("Grade"))
        {
            return currentPrisonerInfo["Grade"].ToString();
        }
        else return null;
    }

    public string GetCrimeGrade()
    {
        if (currentPrisonerInfo.ContainsKey("CrimeGrade"))
        {
            return currentPrisonerInfo["CrimeGrade"].ToString();
        }
        else return null;
    }

    public string GetCrimeReason()
    {
        if(currentPrisonerInfo.ContainsKey("CrimeReason"))
        {
            return currentPrisonerInfo["CrimeReason"].ToString();
        }
        return null;
    }

    public string GetAttackerMove()
    {
        if (currentPrisonerInfo.ContainsKey("AttackerMove"))
        {
            return currentPrisonerInfo["AttackerMove"].ToString();
        }
        return null;
    }

    public string GetVictimMove()
    {
        if (currentPrisonerInfo.ContainsKey("VictimMove"))
        {
            return currentPrisonerInfo["VictimMove"].ToString();
        }
        return null;
    }

    public string GetAttackerJob()
    {
        if (currentPrisonerInfo.ContainsKey("AttackerJob"))
        {
            return currentPrisonerInfo["AttackerJob"].ToString();
        }
        return null;
    }

    public string GetVictimJob()
    {
        if (currentPrisonerInfo.ContainsKey("VictimJob"))
        {
            return currentPrisonerInfo["VictimJob"].ToString();
        }
        return null;
    }

    public string GetVictimGrade()
    {
        if (currentPrisonerInfo.ContainsKey("VictimGrade"))
        {
            return currentPrisonerInfo["VictimGrade"].ToString();
        }
        return null;
    }

    public string GetCrimeRecord()
    {
        if (currentPrisonerInfo.ContainsKey("CrimeRecord"))
        {
            return currentPrisonerInfo["CrimeRecord"].ToString();
        }
        else return null;
    }

    public string GetLie()
    {
        if (currentPrisonerInfo.ContainsKey("Lie"))
        {
            return currentPrisonerInfo["Lie"].ToString();
        }
        return null;
    }

    public string GetInfoError()
    {
        if (currentPrisonerInfo.ContainsKey("InfoError"))
        {
            return currentPrisonerInfo["InfoError"].ToString();
        }
        return null;
    }

    public string GetAsk()
    {
        if (currentPrisonerInfo.ContainsKey("Ask"))
        {
            return currentPrisonerInfo["Ask"].ToString();
        }
        else return null;
    }

    public int getAnswer()
    {
        int answer = 1;
        if (currentPrisonerInfo.ContainsKey("Answer"))
        {
            answer = int.Parse(currentPrisonerInfo["Answer"].ToString());
        }
        return answer;
    }
}
