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


    /// positionGrade = 신분(시민 등급)
    void Awake()
    {
        if (nameT == null)
        {
            nameT = csvReader.Read("nameInfo");
            fnameT = csvReader.Read("familyNameInfo");
            crimeT = csvReader.Read("crimeInfo");
            detailT = csvReader.Read("crimeReasonInfo");
            jobT = csvReader.Read("jobInfo");

            string fileName = "DayJudgeMentInfo";
            //judgeT.Add(CSVReader.Read(fileName + HangingManager.day.ToString()));

            judgeT.Add(csvReader.Read("1" + fileName));
            judgeT.Add(csvReader.Read("2" + fileName));
            judgeT.Add(csvReader.Read("3" + fileName));
            judgeT.Add(csvReader.Read("4" + fileName));
            judgeT.Add(csvReader.Read("5" + fileName));
            judgeT.Add(csvReader.Read("6" + fileName));
            judgeT.Add(csvReader.Read("7" + fileName));
        }
    }

    public Dictionary<string,string> GetData(string attackerFamilyName, string attackerName, ref Dictionary<string, List<string>> lieORInfoError)
    {
        Dictionary<string,string> data = new Dictionary<string,string>();
        Debug.Log("day : " + HangingManager.day);

        GetPositionGradeAndFamilyName(data);
        GetName(nameT, data);
        data["age"] = GetAge();
        if (data["familyName"].Equals(attackerFamilyName))
        {
            while (data["name"].Equals(attackerName) == false) 
                GetName(nameT, data);
        }
        
        data["move"] = data["crimePlace"].Equals(data["familyGrade"]) ? "1" :"0";


        //통합 기록일 경우 가해자 data에만 넣음//
        if (attackerFamilyName == null)
        {
            GetcrimeRecord(data);
            GetCrimeAndCrimeGrade(data);
            GetCrimeReason(data, data["crime"]);
            data["job"] = GetJob(data, data["positionGrade"], "attacker"); Debug.Log("Job : " + data["job"]);

            //위증여부//
            if(HangingManager.day >= 6) 
                GetLieORInfoError(data, ref lieORInfoError);
        }
        else
        {
            data["job"] = GetJob(data, data["positionGrade"], "victim"); Debug.Log("Job : " + data["job"]);
        }

        return data;
    }

    void GetCrimeAndCrimeGrade(Dictionary<string, string> data)
    {
        while (true)
        {
            int valueid = Random.Range(0, crimeT.Count);
            int crimeGrade = getRandomValueByRange(new int[] {15,25,30,20,10});

            string str = crimeT[valueid][crimeT[0]["header"][crimeGrade]][0];
            if (str.Equals("") == false){
                data["crime"] = str;
                data["crimeGrade"] = (int.Parse(crimeT[0]["header"][crimeGrade][0].ToString()) - 1).ToString();

                return;
            }
        }
    }

    void GetCrimePlace(Dictionary<string, string> data, string attackerName) 
    {
        if (HangingManager.day == 1)
        {
            data["crimePlace"] = data["familyGrade"];
        }
        else
        {
            int moveRandomValue;    // 0:불법이동
            if (attackerName == null)
                moveRandomValue = getRandomValueByRange(new int[] { 30, 70 });
            else
                moveRandomValue = getRandomValueByRange(new int[] { 20, 80 });

            if (moveRandomValue == 0)
            {
                do
                    data["crimePlace"] = Random.Range(0, 7).ToString();
                while (data["crimePlace"].Equals(data["familyGrade"]));
            }
            else
            {
                data["crimePlace"] = data["familyGrade"];
            }
        }

        data["crimePlaceText"] = GetCrimePlace(data["crimePlace"]);
    }

    void GetPositionGradeAndFamilyName(Dictionary<string, string> data)
    {
        int positionGrade = getRandomValueByRange(new int[] { 5, 15, 30, 30, 20 });
        data["positionGrade"] = positionGrade.ToString();
        data["familyGrade"] = GetFamilyGrade(data["positionGrade"]);

        
        data["familyName"] = fnameT[Random.Range(0, fnameT.Count)][fnameT[0]["header"][positionGrade]][0];
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
    private string GetFamilyGrade(string grade)
    {
        switch (grade)
        {
            case "0":
                return "6";
            case "1":
                return Random.Range(4, 6).ToString();
            case "2":
                return Random.Range(2, 4).ToString();
            case "3":
                return "1";
            case "4":
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

        for (int i = 0; i < 4; i++)
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
                if (!lieORInfoError.ContainsKey("lie")) lieORInfoError.Add("lie", new List<string>());
                lieORInfoError["lie"].Add(strs[i]);
            }

            //정보 오류//
            else
            {
                if (HangingManager.day >= 7)
                {
                    if (!lieORInfoError.ContainsKey("infoError")) lieORInfoError.Add("infoError", new List<string>());
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
    public string GetRandomStatement(string str)
    {
        if (str == "nameT")
        {
            int valueid = Random.Range(0, nameT.Count);
            int headerid = Random.Range(0, nameT[0]["header"].Count);

            return nameT[valueid][nameT[0]["header"][headerid]][0];
        }
        else if (str == "fnameT")
        {
            int valueid = Random.Range(0, fnameT.Count);
            int headerid = Random.Range(0, fnameT[0]["header"].Count);

            return fnameT[valueid][fnameT[0]["header"][headerid]][0];
        }
        else if (str == "crime")
        {
            int valueid = Random.Range(0, crimeT.Count);
            int headerid = Random.Range(0, crimeT[0]["header"].Count);

            return crimeT[valueid][crimeT[0]["header"][headerid]][0];
        }
        else if (str == "crimePlaceText")
        {
            return GetCrimePlace(Random.Range(0, 7).ToString());
        }
        else //마지막 crimeReasonText
        {
            int valueid = Random.Range(0, crimeT.Count);
            int headerid = Random.Range(0, crimeT[0]["header"].Count);

            return detailT[valueid][detailT[0]["header"][0]][0];
        }
    }

    int getRandomValueByRange(int[] randomValueRange)
    {
        int sumRange = 0;
        int randomValue = Random.Range(1, 101);

        int randomValueRangeSize = randomValueRange.Length;
        for (int index = 0; index < randomValueRangeSize; ++index)
        {
            sumRange += randomValueRange[index];
            if (randomValue <= sumRange)
                return index;
        }

        Debug.Log("확률 총합이 " + randomValue + "입니다. 수정이 필요합니다.");
        return -1;
    }
}