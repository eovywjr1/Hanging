using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerInfoTable : MonoBehaviour
{
    ReadPrisonerInfo readPrisonerInfo;

    int day = HangingManager.day;

    private void Awake()
    {
        readPrisonerInfo = FindObjectOfType<ReadPrisonerInfo>();
    }

    public Dictionary<string, string> GetData()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        data["positionGrade"] = readPrisonerInfo.currentPrisonerInfo["Grade"];
        data["crimeGrade"] = readPrisonerInfo.currentPrisonerInfo["CrimeGrade"];

        GetByDay(data, day, "attacker");    //attacker은 임시

        return data;
    }

    void GetByDay(Dictionary<string, string> data, int day, string personType) //personType은 사형수인지, 피해자인지
    {
        if(day>=2)
        {
            data["crimeReason"] = readPrisonerInfo.GetCrimeReason();
            if(personType == "attacker")
            {
                data["move"] = readPrisonerInfo.GetAttackerMove();
            }
            else
            {
                data["move"] = readPrisonerInfo.GetVictimMove();
            }
        }

        if(day>=3)
        {
            if(personType == "attacker")
            {
                data["job"] = readPrisonerInfo.GetAttackerJob();
            }
            else
            {
                data["job"] = readPrisonerInfo.GetVictimJob();
            }

            data["ask"] = readPrisonerInfo.GetAsk();
        }

        if(day>=2)
        {
            data["victimGrade"] = readPrisonerInfo.GetVictimGrade();
        }
    }
}
