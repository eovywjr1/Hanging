using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    CSVReader csvReader = new CSVReader();
    private static List<Dictionary<string, List<string>>> nameT, fnameT, crimeT, detailT, jobT;
    public static List<List<Dictionary<string, List<string>>>> judgeT = new List<List<Dictionary<string, List<string>>>>();

    void Awake()
    {
        if (nameT == null)
        {
            nameT = csvReader.Read("사건기록서 이름");
            fnameT = csvReader.Read("사건기록서 성");
            crimeT = csvReader.Read("사건기록서 죄명");
            detailT = csvReader.Read("사건기록서 경위");
            jobT = csvReader.Read("사건기록서 직업");

            string fileName = "사건기록서 판단";
            //judgeT.Add(CSVReader.Read(fileName + HangingManager.day.ToString()));

            judgeT.Add(csvReader.Read(fileName + "1"));
            judgeT.Add(csvReader.Read(fileName + "2"));
            judgeT.Add(csvReader.Read(fileName + "3"));
            judgeT.Add(csvReader.Read(fileName + "4"));
            judgeT.Add(csvReader.Read(fileName + "5"));
            judgeT.Add(csvReader.Read(fileName + "6"));
            judgeT.Add(csvReader.Read(fileName + "7"));
        }
    }

    public Dictionary<string,string> GetData(string familyName, string name, ref Dictionary<string, List<string>> lieORInfoError)
    {
        Dictionary<string,string> data = new Dictionary<string,string>();
        Debug.Log("day : " + HangingManager.day);
        
        GetFamilyName(data);
        GetName(nameT, data);
        data["age"] = GetAge();
        if (data["familyName"].Equals(familyName))
        {
            while (!data["name"].Equals(name)) GetName(nameT, data);
        }
        
        data["move"] = data["crimePlace"].Equals(data["familyGrade"]) ? "1" :"0";


        //통합 기록일 경우 가해자 data에만 넣음//
        if (familyName == null)
        {
            GetcrimeRecord(data);
            GetCrime(data);
            GetCrimeReason(data, data["crime"]);
            data["job"] = GetJob(data, data["positionGrade"], "attacker"); Debug.Log("Job : " + data["job"]);

            //위증여부//
            if(HangingManager.day >= 6) GetLieORInfoError(data, ref lieORInfoError);
        }
        else
        {
            data["job"] = GetJob(data, data["positionGrade"], "victim"); Debug.Log("Job : " + data["job"]);
        }

        return data;
    }

    private void GetCrime(Dictionary<string, string> data)
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
        int headerid = Random.Range(0, fnameT[0]["header"].Count);  //5
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

    private void GetName(List<Dictionary<string, List<string>>> list, Dictionary<string, string> data)
    {
        int valueid = Random.Range(0, list.Count);
        int headerid = Random.Range(0, list[0]["header"].Count);

        data["gender"] = list[0]["header"][headerid];
        data["name"] = list[valueid][list[0]["header"][headerid]][0];
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
    //person은 가해자, 피해자 구분
    string GetJob(Dictionary<string, string> data, string positionGrade, string person)
    {
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
        data["jobText"] = job;
        Debug.Log("직업 : " + job);
        switch (HangingManager.day)
        {
            case 3:
                if (job.Equals("의사") || job.Equals("연구원") || job.Equals("기술자")) return "1";
                else return "0";
            case 4:
                if (person.Equals("attacker"))
                {
                    if ((job.Equals("개발자") && data["positionGrade"].Equals("2")) || (job.Equals("교사") && int.Parse(data["positionGrade"]) >= 3)) return "2";
                    else if (job.Equals("의사") || job.Equals("연구원") || job.Equals("기술자") || job.Equals("개발자") || job.Equals("교사")) return "1";
                    else return "0";
                }
                else
                {
                    if (job.Equals("의사") || job.Equals("연구원") || job.Equals("기술자") || job.Equals("개발자") || job.Equals("교사")) return "1";
                    else return "0";
                }
            case 5:
            case 6:
                switch (job)
                {
                    case "상담가": return "5";
                    case "교사": return "4";
                    case "개발자":
                        if(data["positionGrade"].Equals("2")) return "3";
                        break;
                    case "연구원": return "2";
                    case "의사": return "1";
                    default: return "0";
                }
                return null;
            case 7:
                switch (job)
                {
                    case "농업기술자": return "6";
                    case "연구원":
                        if (int.Parse(data["positionGrade"]) >= 3) return "5";
                        break;
                    case "기술자": return "4";
                    case "교도관": return "3";
                    case "의사": return "2";
                    case "교사": return "1";
                    default: return "0";
                }
                return null;
            default:
                return null;
        }
    }
    
    string GetAge()
    {
        return Random.Range(20, 61).ToString();
    }

    void GetcrimeRecord(Dictionary<string, string> data)
    {
        if (HangingManager.day >= 4)
        {
            data["crimeRecord"] = Random.Range(0, 6).ToString();
            if (int.Parse(data["crimeRecord"]) == 0)
            {
                data["crimeRecordText"] = "없음";
                return;
            }
            while (true)
            {
                int valueid = Random.Range(0, crimeT.Count);
                int headerId = int.Parse(data["crimeRecord"]) - 1;

                string str = crimeT[valueid][crimeT[0]["header"][headerId]][0];
                if (!str.Equals(""))
                {
                    data["crimeRecordText"] = str;
                    return;
                }
            }
        }
    }

    void GetLieORInfoError(Dictionary<string, string> data, ref Dictionary<string, List<string>> lieORInfoError)
    {
        string[] strs = { "name", "crime", "crimePlace", "crimeResonText" };
        bool crimePlaceFlag = false, lieFlag = false;
        int cnt = 0;

        for (int i = 0; i < 5; i++)
        {
            int lieORInfoErrorPossibility = Random.Range(0, 5);
            if (lieORInfoErrorPossibility != 0) continue;

            if (i == 2) crimePlaceFlag = true;
            cnt++;

            int lieORInfoErrorDistinguishPossibility = Random.Range(0, 2);
            //6일차 무조건 위증//
            if (HangingManager.day == 6) lieORInfoErrorDistinguishPossibility = 0;
            //위증//
            if (lieORInfoErrorDistinguishPossibility == 0)
            {
                lieFlag = true;
                if (!lieORInfoError.ContainsKey("lie")) lieORInfoError["lie"] = new List<string>();
                lieORInfoError["lie"].Add(strs[i]);
            }

            //정보 오류//
            else
            {
                if (HangingManager.day >= 7)
                {
                    if (!lieORInfoError.ContainsKey("infoError")) lieORInfoError["infoError"] = new List<string>();
                    lieORInfoError["infoError"].Add(strs[i]);
                }
            }
        }

        if (cnt >= 3)
        {
            if (crimePlaceFlag) data["infoError"] = "2";
            else data["infoError"] = "0";
        }
        else data["infoError"] = "1";

        data["lie"] = lieFlag ? "0" : "1";
    }
}