using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    private static List<Dictionary<string, List<string>>> nameT, fnameT, crimeT, detailT, jobT;
    public static List<List<Dictionary<string, List<string>>>> judgeT = new List<List<Dictionary<string, List<string>>>>();

    void Awake()
    {
        if (nameT == null)
        {
            nameT = CSVReader.Read("사건기록서 이름");
            fnameT = CSVReader.Read("사건기록서 성");
            crimeT = CSVReader.Read("사건기록서 죄명");
            detailT = CSVReader.Read("사건기록서 경위");
            jobT = CSVReader.Read("사건기록서 직업");

            string fileName = "사건기록서 판단";
            //judgeT.Add(CSVReader.Read(fileName + HangingManager.day.ToString()));

            judgeT.Add(CSVReader.Read(fileName + "1"));
            judgeT.Add(CSVReader.Read(fileName + "2"));
            judgeT.Add(CSVReader.Read(fileName + "3"));
        }
    }

    public Dictionary<string,string> GetData(string familyName, string name)
    {
        Dictionary<string,string> data = new Dictionary<string,string>();
        
        GetFamilyName(data);
        data["name"] = GetString(nameT);
        if (data["familyName"].Equals(familyName))
        {
            while (!data["name"].Equals(name)) data["name"] = GetString(nameT);
        }
        Getcrime(data);
        GetCrimeReason(data, data["crime"]);
        data["move"] = "1";//data["crimePlace"].Equals(data["familyGrade"]) ? "1" :"0";
        data["job"] = GetJob(data, data["positionGrade"]);
        Debug.Log("Job : " + data["job"]);

        return data;
    }

    private void Getcrime(Dictionary<string, string> data)
    {
        while (true)
        {
            int valueid = Random.Range(0, crimeT.Count);
            int headerid = Random.Range(0, crimeT[0]["header"].Count);

            string str = crimeT[valueid][crimeT[0]["header"][headerid]][0];
            if (!str.Equals("")){
                data["crime"] = str;
                data["crimeGrade"] = (int.Parse(crimeT[0]["header"][headerid][0].ToString()) - 1).ToString();

                return;
            }
        }
    }

    private void GetFamilyName(Dictionary<string, string> data)
    {
        int valueid = Random.Range(0, fnameT.Count);
        int headerid = Random.Range(0, fnameT[0]["header"].Count);
        int grade = Random.Range(0, 7);

        if (HangingManager.day == 1) {
            data["familyGrade"] = headerid.ToString();
            data["crimePlace"] = data["familyGrade"];
            data["crimePlaceText"] = GetCrimePlace(data["crimePlace"]);
        }
        else {
            data["familyGrade"] = grade.ToString();
            data["crimePlace"] = Random.Range(0, 7).ToString();
            data["crimePlaceText"] = GetCrimePlace(data["crimePlace"]);
        }
        data["positionGrade"] = GetPositionGrade(data["familyGrade"]);
        data["familyName"] = fnameT[valueid][fnameT[0]["header"][headerid]][0];
    }

    private string GetString(List<Dictionary<string, List<string>>> list)
    {
        int valueid = Random.Range(0, list.Count);
        int headerid = Random.Range(0, list[0]["header"].Count);

        return list[valueid][list[0]["header"][headerid]][0];
    }

    private void GetCrimeReason(Dictionary<string, string> data, string crime)
    {
        string grade = data["crimeGrade"];

        List<int> randomlist = new List<int>();
        for (int i = 0; i < detailT.Count; i++)
        {
            string applyGrade = detailT[i][detailT[0]["header"][1]][0];
            string detail = detailT[i][detailT[0]["header"][0]][0];

            if (applyGrade.Equals("ALL")) randomlist.Add(i);
            else
            {
                foreach (var v in detailT[i][detailT[0]["header"][1]])
                {
                    if (grade.Equals(v))
                    {
                        randomlist.Add(i);
                        break;
                    }
                }
            }
        }

        int valueid = Random.Range(0,randomlist.Count);
        data["crimeReasonText"] = detailT[randomlist[valueid]][detailT[0]["header"][0]][0];
        data["crimeReason"] = randomlist[valueid].ToString();
    }

    private string GetCrimePlace(string grade)
    {
        switch (grade)
        {
            case "0":
                return "A";
            case "1":
                return "B";
            case "2":
                return "C";
            case "3":
                return "D";
            case "4":
                return "E";
            case "5":
                return "F";
            case "6":
                return "G";
            default:
                return null;
        }
    }

    //신분 등급//
    private string GetPositionGrade(string grade)
    {
        switch (grade)
        {
            case "0":
                return "4";
            case "1":
                return "3";
            case "2":
            case "3":
                return "2";
            case "4":
            case "5":
                return "1";
            case "6":
                return "0";
            default:
                return null;
        }
    }

    //직업//
    string GetJob(Dictionary<string, string> data, string positionGrade)
    {
        // List<Dictionary<string, List<string>>>
        List<string> jobPossibleList = new List<string>();

        foreach(var i in jobT)
        {
            foreach(var j in i["성 등급"])
            {
                if (j.Equals(positionGrade))
                {
                    jobPossibleList.Add(i["직업"][0]);
                    break;
                }
            }
        }

        string job = jobPossibleList[Random.Range(0, jobPossibleList.Count)];


        //if (job.Equals("의사") || job.Equals("연구원") || job.Equals("기술자")) return "1";
        //else return "0";
        return "1";
    }
}
