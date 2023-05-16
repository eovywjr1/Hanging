using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    CSVReader csvReader = new CSVReader();
    private static WorkProbabilityCSVRedaer workProbabilityCSVRedaer = new WorkProbabilityCSVRedaer();
    private static JobInfoReader jobInfoReader = null;

    private static List<Dictionary<string, List<string>>> nameT, fnameT, crimeT, detailT;
    public static List<List<Dictionary<string, List<string>>>> judgeT = new List<List<Dictionary<string, List<string>>>>();
    private static Dictionary<string, List<List<int>>> probabilityInfo = null;
    private static AcceptCrimeInfoReader acceptCrimeInfoReader = null;
    private static SpecialJobIndexInfoReader specialJobIndexInfoReader = null;

    ReadPrisonerInfo readPrisonerInfo;

    /// positionGrade = 신분(시민 등급)
    void Awake()
    {
        if (nameT == null)
        {
            nameT = csvReader.Read("nameInfo");
            fnameT = csvReader.Read("familyNameInfo");
            crimeT = csvReader.Read("crimeInfo");
            detailT = csvReader.Read("crimeReasonInfo");

            probabilityInfo = workProbabilityCSVRedaer.Read("WorkProbabilityInfo");
            jobInfoReader = new JobInfoReader(Resources.Load("JobInfo") as TextAsset);
            acceptCrimeInfoReader = new AcceptCrimeInfoReader(Resources.Load("AcceptCrimeInfo") as TextAsset);
            specialJobIndexInfoReader = new SpecialJobIndexInfoReader(Resources.Load("AcceptCrimeInfo") as TextAsset);

            // 조민수 : 각 일차 별 테스트 할 때 사용하는 코드 //
            string fileName = "DayJudgeMentInfo";
            judgeT.Add(csvReader.Read("1" + fileName));
            judgeT.Add(csvReader.Read("2" + fileName));
            judgeT.Add(csvReader.Read("3" + fileName));
            judgeT.Add(csvReader.Read("4" + fileName));
            judgeT.Add(csvReader.Read("5" + fileName));
            judgeT.Add(csvReader.Read("6" + fileName));
            judgeT.Add(csvReader.Read("7" + fileName));
            judgeT.Add(csvReader.Read("8" + fileName));
        }

        // 조민수 : 정상적인 플레이에서는 각 해당 일차마다 추가 //
        //string fileName = "DayJudgeMentInfo";
        //judgeT.Add(CSVReader.Read(fileName + HangingManager.day.ToString()));

        readPrisonerInfo = FindObjectOfType<ReadPrisonerInfo>();
    }

    public Dictionary<string,string> GetData(string attackerFamilyName, string attackerName, ref Dictionary<string, List<string>> lieORInfoError)
    {
        Dictionary<string,string> data = new Dictionary<string,string>();
        Debug.Log("day : " + HangingManager.day);

        if (attackerFamilyName == null)
        {
            data["move"] = readPrisonerInfo.GetAttackerMove();
        }
        else
        {
            data["move"] = readPrisonerInfo.GetVictimMove();
        }

        GetPositionGradeAndFamilyName(data);
        GetName(nameT, data);
        data["age"] = GetAge();
        if (data["familyName"].Equals(attackerFamilyName))
        {
            while (data["name"].Equals(attackerName) == false) 
                GetName(nameT, data);
        }
        GetCrimePlaceAndMove(data, attackerName);

        //통합 기록일 경우 가해자 data에만 넣음//
        if (attackerFamilyName == null)
        {
            GetcrimeRecord(data);
            SetCrimeAndCrimeGrade(data);
            SetCrimeReason(data);
            SetJob(data, data["positionGrade"], 1);
            Debug.Log("Job : " + data["job"]);

            //위증여부//
            if(HangingManager.day >= 6) 
                GetLieORInfoError(data, ref lieORInfoError);

            //국가적 요구 허락OR거절
            data["ask"] = getRandomValueByRange(probabilityInfo["askAccept"][0]).ToString();
        }
        else
        {
            SetJob(data, data["positionGrade"], 0); 
            Debug.Log("Job : " + data["job"]);
        }

        return data;
    }

    void SetCrimeAndCrimeGrade(Dictionary<string, string> data)
    {
        while (true)
        {
            int valueid = Random.Range(0, crimeT.Count);
            int crimeGrade = getRandomValueByRange(probabilityInfo["crimeGrade"][0]);

            string str = crimeT[valueid][crimeT[0]["header"][crimeGrade]][0];
            if (str.Equals("") == false){
                data["crime"] = str;

                data["crimeGrade"] = readPrisonerInfo.GetCrimeGrade();
                if (data["crimeGrade"] != null) Debug.Log("쒣아님");
                if (data["crimeGrade"] == null)
                {
                    data["crimeGrade"] = (int.Parse(crimeT[0]["header"][crimeGrade][0].ToString()) - 1).ToString();
                }
                /*data["crimeGrade"] = (int.Parse(crimeT[0]["header"][headerid][0].ToString()) - 1).ToString();*/

                //data["crimeGrade"] = (int.Parse(crimeT[0]["header"][crimeGrade][0].ToString()) - 1).ToString();

                return;
            }
        }
    }

    void GetCrimePlaceAndMove(Dictionary<string, string> data, string attackerName) 
    {
        int moveFlag = 0;

        if (HangingManager.day == 1)
        {
            data["crimePlace"] = data["familyGrade"];
        }
        else
        {
            if (attackerName == null)
                moveFlag = getRandomValueByRange(probabilityInfo["attackerMove"][0]);
            else
                moveFlag = getRandomValueByRange(probabilityInfo["victimMove"][0]);

            if (moveFlag == 1)
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

        data["crimePlaceText"] = GetCrimePlaceText(data["crimePlace"]);
        data["move"] = (moveFlag == 1) ? "1" : "0";
    }

    void GetPositionGradeAndFamilyName(Dictionary<string, string> data)
    {
        int positionGrade = getRandomValueByRange(probabilityInfo["positionGrade"][0]);
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

    private void SetCrimeReason(Dictionary<string, string> data)
    {
        const int acceptCrimeProbability = 30;
        const int maxCrimeReasonIndex = 4;
        int isAcceptCrime = 0;
        List<int> acceptCrimeList = acceptCrimeInfoReader.getAccpetCrimeDictionary(HangingManager.day);

        if (acceptCrimeList != null)
            isAcceptCrime = getRandomValueByRange(new List<int> { 100 - acceptCrimeProbability, acceptCrimeProbability });

        if (isAcceptCrime == 1)
        {
            data["crimeReason"] = acceptCrimeList[UnityEngine.Random.Range(0, acceptCrimeList.Count)].ToString();
        }
        else
        {
            if(acceptCrimeList != null)
            {
                acceptCrimeList.Sort();
                List<int> unAcceptList = new List<int>();
                for (int crimeReasonIndex = 0, acceptCrimeListIndex = 0; crimeReasonIndex <= maxCrimeReasonIndex; ++crimeReasonIndex)
                {
                    if (acceptCrimeList[acceptCrimeListIndex] != crimeReasonIndex)
                        unAcceptList.Add(crimeReasonIndex);
                    else if (acceptCrimeList[acceptCrimeListIndex] <= crimeReasonIndex)
                        ++acceptCrimeListIndex;
                }

                data["crimeReason"] = unAcceptList[UnityEngine.Random.Range(0, unAcceptList.Count)].ToString();
            }
            else
            {
                data["crimeReason"] = UnityEngine.Random.Range(0, maxCrimeReasonIndex).ToString();
            }
        }
        data["crimeReasonText"] = detailT[int.Parse(data["crimeReason"])][detailT[0]["header"][0]][0];
    }

    private string GetCrimePlaceText(string grade)
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
    void SetJob(Dictionary<string, string> data, string positionGrade, int isAttacker)
    {
        int specialJobFlag = 0;
        int day = HangingManager.day;
        string jobText = null;

        if (isAttacker == 1)
            specialJobFlag = getRandomValueByRange(probabilityInfo["attackerSpecialJob"][int.Parse(data["positionGrade"])]);
        else
            specialJobFlag = getRandomValueByRange(probabilityInfo["victimSpecialJob"][0]);

        if (jobInfoReader._specialJobDictionary.ContainsKey(day))
        {
            if (specialJobFlag == 1)
                jobText = jobInfoReader.getSpecialJob(day);
            else
                jobText = jobInfoReader.getNormalJob(day);
        }
        else
        {
            jobText = jobInfoReader.getJobByAllList();
        }


        //아래 내용을 데이터로 빼야 함
        data["jobText"] = jobText;
        data["job"] = specialJobIndexInfoReader.getSpecialJobIndex(HangingManager.day, jobText, int.Parse(data["positionGrade"]), isAttacker).ToString();

        Debug.Log("직업 : " + jobText);
    }
    
    string GetAge()
    {
        return Random.Range(20, 61).ToString();
    }

    void GetcrimeRecord(Dictionary<string, string> data)
    {
        if (HangingManager.day >= 4)
        {
            if(data["positionGrade"].Equals("0"))
                data["crimeRecord"] = getRandomValueByRange(probabilityInfo["crimeRecordHighest"][0]).ToString();
            else
                data["crimeRecord"] = getRandomValueByRange(probabilityInfo["crimeRecordNotHighest"][0]).ToString();

            if (int.Parse(data["crimeRecord"]) == probabilityInfo["crimeRecordHighest"][0].Count - 1)
            {
                data["crimeRecordText"] = "없음";
            }
            else
            {   
                // 비어있는거 저장안하게 바꿔야 됨.
                while (true)
                {
                    int valueid = Random.Range(0, crimeT.Count);
                    int headerId = int.Parse(data["crimeRecord"]);

                    string str = crimeT[valueid][crimeT[0]["header"][headerId]][0];
                    if (str.Equals("") == false)
                    {
                        data["crimeRecordText"] = str;
                        return;
                    }
                }
            }
        }
    }

    void GetLieORInfoError(Dictionary<string, string> data, ref Dictionary<string, List<string>> lieORInfoError)
    {
        string[] errorList = { "fullName", "crime", "crimePlace", "crimeResonText" };
        bool isFullNameError = false, isLie = false;
        int cnt = 0;

        if (getRandomValueByRange(probabilityInfo["lieORInfoErrorAppear"][0]).Equals("1")){
            int errorListLength = errorList.Length;
            for (int i = 0; i < errorListLength; ++i)
            {
                int lieORInfoErrorPossibility = Random.Range(0, 2);
                if (lieORInfoErrorPossibility == 0) 
                    continue;

                if (i == 0) 
                    isFullNameError = true;

                cnt++;

                int lieORInfoErrorDistinguish = getRandomValueByRange(probabilityInfo["lieORInfoError"][0]);
                //6일차 무조건 위증//
                if (HangingManager.day == 6)
                    lieORInfoErrorDistinguish = 0;

                //위증//
                if (lieORInfoErrorDistinguish == 0)
                {
                    isLie = true;
                    if (!lieORInfoError.ContainsKey("lie")) lieORInfoError.Add("lie", new List<string>());
                    lieORInfoError["lie"].Add(errorList[i]);
                    data["lie"] = isLie ? "1" : "0";
                }
                //정보 오류//
                else
                {
                    if (HangingManager.day >= 7)
                    {
                        if (!lieORInfoError.ContainsKey("infoError")) lieORInfoError.Add("infoError", new List<string>());
                        lieORInfoError["infoError"].Add(errorList[i]);
                    }
                }
            }
        }

        if (cnt >= 3)
        {
            if (isFullNameError) 
                data["infoError"] = "2";
            else 
                data["infoError"] = "0";
        }
        else
        {
            data["infoError"] = "1";
        }

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
            return GetCrimePlaceText(Random.Range(0, 7).ToString());
        }
        else //마지막 crimeReasonText
        {
            int valueid = Random.Range(0, crimeT.Count);
            int headerid = Random.Range(0, crimeT[0]["header"].Count);

            return detailT[valueid][detailT[0]["header"][0]][0];
        }
    }

    int getRandomValueByRange(List<int> randomValueRange)
    {
        int sumRange = 0;
        int randomValue = Random.Range(1, 101);

        int randomValueRangeCount = randomValueRange.Count;
        for (int index = 0; index < randomValueRangeCount; ++index)
        {
            sumRange += randomValueRange[index];
            if (randomValue <= sumRange)
                return index;
        }

        Debug.LogWarning($" 확률 총합이 '{randomValue}' 입니다. 수정이 필요합니다.");
        return -1;
    }
}