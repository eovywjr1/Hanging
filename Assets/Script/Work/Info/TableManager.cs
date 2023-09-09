using System.Collections.Generic;
using System.Security.Cryptography;
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
    WorkSystemData systemDataManager;    //유전자코드, 몸수색, 얼굴 수색 등의 시스템 정보 관리

    bool isApplySpecificInfo;   // 특정 정보 적용 여부

    /// positionGrade = 신분(시민 등급)
    void Awake()
    {
        if (HangingManager.day >= 1 && HangingManager.day <= 7)
            isApplySpecificInfo = true;
        else
            isApplySpecificInfo = false;

        if (nameT == null)
        {
            nameT = csvReader.Read("nameInfo");
            fnameT = csvReader.Read("familyNameInfo");
            crimeT = csvReader.Read("crimeInfo");
            detailT = csvReader.Read("crimeReasonInfo");

            probabilityInfo = workProbabilityCSVRedaer.Read("WorkProbabilityInfo");
            jobInfoReader = new JobInfoReader(Resources.Load("JobInfo") as TextAsset);
            acceptCrimeInfoReader = new AcceptCrimeInfoReader(Resources.Load("AcceptCrimeInfo") as TextAsset);
            specialJobIndexInfoReader = new SpecialJobIndexInfoReader(Resources.Load("SpecialJobIndexInfo") as TextAsset);

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
            judgeT.Add(csvReader.Read("9" + fileName));
            judgeT.Add(csvReader.Read("10" + fileName));
            zz
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

            //9일차부터 복제인간 등장
            if (HangingManager.day >= 9)
            {
                systemDataManager = FindObjectOfType<WorkSystemData>();

                SetHumanClone(data);
            }

            if (HangingManager.day >= 12)
            {
                SetCrimeType(data);         //범죄 종류 정의
                SetGeneCode(data);          //유전자 코드 발견 여부 정의
                SetNormalGeneCode(data);    //정상인 유전자 코드 여부 정의
                SetGradeBrand(data);        //몸수색 결과 시민 등급 표식의 이상을 정의
            }

            if(HangingManager.day >= 13)
            {
                SetGradeBrandChange(data);  //몸수색 결과 중 시민 등급 표식 변경일 정의
                SetScar(data);  //몸수색 결과 중 흉터 여부 정의
                SetTattoo(data);    //몸수색 결과 중 문신 여부 정의
                SetBrandChange(data);      // 시민 등급 표식 사유
            }

            if (HangingManager.day >= 14)
            {
                if (data["scar"] == "1")    //화상 흉터가 있을 경우
                {
                    SetBurn(data);  //몸수색 결과 중 화상 흉터와 출생일 사이의 관계를 정의
                }
                if (data["brandChange"] == "4" || data["brandChange"] == "5")
                {
                    // 시민 등급 표식이 결혼이나 입적으로 변경된 경우,
                    // 배우자나 부모의 시민등급과 본인의 시민 등급 사이의 관계를 정의
                    SetPartnerGrade(data);
                }
            }
            if (HangingManager.day >= 15)
            {
                if (data["tatto"] == "2")
                    SetRTattoDate(data);
                SetFace(data);

            }

            if(HangingManager.day >= 17)
            {
                SetPupil(data);
                SetHair(data);
            }
            
            if (HangingManager.day >= 18)
            {
                SetUnderTatto(data);
            }

            if (HangingManager.day >= 19)
            {
                SetDegradation(data);
            }

            //국가적 요구 허락OR거절     //유민 수정
            data["askAccept"] = ((isApplySpecificInfo) && (readPrisonerInfo.GetAsk() != null)) ? readPrisonerInfo.GetAsk() : getRandomValueByRange(probabilityInfo["askAccept"][0]).ToString();
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

            //유민 수정
            int crimeGrade;
            if(isApplySpecificInfo && readPrisonerInfo.GetCrimeGrade() != -1)
            {
                crimeGrade = readPrisonerInfo.GetCrimeGrade();
            }
            else
            {
                crimeGrade = getRandomValueByRange(probabilityInfo["crimeGrade"][0]);
            }

            string str = crimeT[valueid][crimeT[0]["header"][crimeGrade]][0];
            if (str.Equals("") == false){
                data["crime"] = str;
                data["crimeGrade"] = (int.Parse(crimeT[0]["header"][crimeGrade][0].ToString()) - 1).ToString();

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
            //유민 수정
            if (attackerName == null)
            {
                if (isApplySpecificInfo && readPrisonerInfo.GetAttackerMove() != -1)
                    moveFlag = readPrisonerInfo.GetAttackerMove();
                else
                    moveFlag = getRandomValueByRange(probabilityInfo["attackerMove"][0]);
            }
            else
            {
                if (isApplySpecificInfo && readPrisonerInfo.GetVictimMove() != -1)
                    moveFlag = readPrisonerInfo.GetVictimMove();
                else
                    moveFlag = getRandomValueByRange(probabilityInfo["victimMove"][0]);

            }

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
        //유민 수정
        int positionGrade;
        if(isApplySpecificInfo && readPrisonerInfo.GetGrade() != -1)
        {
            positionGrade = readPrisonerInfo.GetGrade();    
        }
        else
        {
            positionGrade = getRandomValueByRange(probabilityInfo["positionGrade"][0]);
        }
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

    private void SetHumanClone(Dictionary<string, string> data)
    {
        if(systemDataManager.isClone)
        {
            data["humanClone"] = 1.ToString();  //복제인간일 경우
        }
        else
        {
            data["humanClone"] = 0.ToString();  //복제인간 아닐 경우
        }
    }

    private void SetCrimeType(Dictionary<string, string> data)
    {
        data["crimeType"] = Random.Range(0, 5).ToString();
    }

    private void SetGeneCode(Dictionary<string, string> data)
    {
        bool foundGeneCode = false; //유전자 코드 발견 작업 구현 완료 후 수정 필요

        if (foundGeneCode)
            data["geneCode"] = "1";
        else
            data["geneCode"] = "0";
    }

    private void SetNormalGeneCode(Dictionary<string, string> data)
    {
        bool foundNormalGeneCode = false;   //유전자 코드 발견 작업 구현 완료 후 수정 필요

        if (foundNormalGeneCode)
            data["normalGeneCode"] = "1";
        else
            data["normalGeneCode"] = "0";
    }

    private void SetGradeBrand(Dictionary<string, string> data)
    {
        //몸수색 작업 구현 완료 후 수정 필요

        bool sameWithBirthday = true;       //등급 표식 생성일 == 출생일 일 경우
        bool sameWithDateChanged = true;    //등급 표식 생성일 == 변경일 일 경우

        if(sameWithBirthday && !sameWithDateChanged)
        {
            data["gradeBrand"] = "1";
        }
        else if(!sameWithBirthday && sameWithDateChanged)
        {
            data["gradeBrand"] = "2";
        }
        else if(!sameWithBirthday && !sameWithDateChanged)
        {
            data["gradeBrand"] = "0";
        }
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
            //유민 수정
            if (isApplySpecificInfo && readPrisonerInfo.GetCrimeReason() != -1)
            {
                data["crimeReason"] = acceptCrimeList[readPrisonerInfo.GetCrimeReason()].ToString();
            }
            else
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

    private void SetGradeBrandChange(Dictionary<string, string> data)
    {
        //몸수색 구현 완료 후 수정 필요
        //시민 등급 변경일이 사유(모범, 국가공헌, 국정 지원)에 따라 다름
        System.DateTime changeDate = System.DateTime.Today; //임시
        System.DateTime baseDate = System.DateTime.Today;   //임시

        int result = System.DateTime.Compare(changeDate, baseDate);
    
        if(result < 0)  //시민등급 변경일이 해당 '변경 제도 시행일' 이전
        {
            data["gradeBrandChange"] = "0";
        }
        else //시민등급 변경일이 해당 '변경 제도 시행일' 이후인 경우
        {
            data["gradeBrandChange"] = "1";
        }
    }

    private void SetScar(Dictionary<string, string> data)
    {
        // 몸수색 구현 완료 후 수정 필요
        int scar = 0;   //몸수색 관련 스크립트에서 흉터 여부 값 가져오기

        data["scar"] = scar.ToString();
    }

    private void SetTattoo(Dictionary<string, string> data)
    {
        //몸수색 구현 완료 후 수정 필요
        int tattoo = 0; //임시값, 몸수색 관련 스크립트에서 타투 여부 값 가져오기
        data["tattoo"] = tattoo.ToString();
    }

    private void SetBrandChange(Dictionary<string, string> data)
    {
        if (HangingManager.day == 13)
            data["brandChange"] = Random.Range(0, 4).ToString();
        else
            data["brandChange"] = Random.Range(0, 6).ToString();
    }

    // 혜원 추가
    private void SetBurn(Dictionary<string, string> data)
    {
        //몸수색 구현 완료 후 수정 필요
        int burn = 0;   //몸수색 관련 스크립트에서 화상 여부 값 가져오기
        data["burn"] = burn.ToString();
    }
    private void SetPartnerGrade(Dictionary<string, string> data)
    {
        // 시민 등급 표식 변경자 중 사유가 결혼 또는 입적인 경우,
        // 배우자와 부모의 시민 등급을 정의

        int partnerGrade = 0; //임시값, 부모나 배우자의 등급이 저장된다고 생각.
        //int parentGrade = 0; //임시값, 혹시나 따로 저장될 경우.

        if (int.Parse(data["positionGrade"]) == partnerGrade)
        {
            data["partnerGrade"] = "0";
        }
        if (int.Parse(data["positionGrade"]) < partnerGrade)
        {
            data["partnerGrade"] = "1";
        }
        if (int.Parse(data["positionGrade"]) > partnerGrade)
        {
            data["partnerGrade"] = "2";
        }
    }

    private void SetRTattoDate(Dictionary<string, string> data)
    {
        //몸수색 구현 완료 후 수정 필요
        //캐릭터 몸수색 결과 중 반란군 문신 생성일을 정의

        int rtattoDate = 0; //임시 값, 몸수색 관련 스크립트에서
                            //반란군 문신 생성일 값 가져오기

        if (rtattoDate <= 2011)
        {
            data["rTattoDate"] = "0";
        }
        else if (rtattoDate >= 2012 && rtattoDate <= 2016)
        {
            data["rTattoDate"] = "1";
        }
        else
        {
            data["rTattoDate"] = "2";
        }
    }

    private void SetFace(Dictionary<string, string> data)
    {
        //캐릭터의 얼굴 수색 결과 중
        //캐릭터가 충족한 시민 등급 외형 조건을 정의

        //얼굴 수색 구현 후 수정 필요

        bool ARankCitizenAppearance = false;    //임시값, 최상급
        bool BRankCitizenAppearance = false;    //임시값, 상급
        bool ERankCitizenAppearance = false;    //임시값, 최하급

        if (!ARankCitizenAppearance && !BRankCitizenAppearance && 
            !ERankCitizenAppearance)
        {
            data["face"] = "0";
        }
        else if (ARankCitizenAppearance)
        {
            data["face"] = "1";
        }
        else if (BRankCitizenAppearance)
        {
            data["face"] = "2";
        }
        else if (ERankCitizenAppearance)
        {
            data["face"] = "3";
        }

        //?????? 임시!!
        /*
        FaceSearchData faceSearchData = FindObjectOfType<FaceSearchData>();
        data["face"] = faceSearchData.faceData["faceGrade"].ToString();
        */
    }

    //
    private void SetPupil(Dictionary<string, string> data)
    {
        FaceColorData faceColorData = FindObjectOfType<FaceColorData>();
        data["pupil"] = faceColorData.getEyeColor().ToString();
    }

    private void SetHair(Dictionary<string, string> data)
    {
        FaceColorData faceColorData = FindObjectOfType<FaceColorData>();
        data["hair"] = faceColorData.getHairColor().ToString();
    }

    private void SetUnderTatto(Dictionary<string, string> data)
    {
        //몸수색 구현 완료 후 수정 필요
        //몸수색 관련 스크립트에서 커버 문신 아래 있는 문신 값 가져오기
        int underTatto = 0;   //임시값
        data["underTatto"] = underTatto.ToString();
    }

    private void SetDegradation(Dictionary<string, string> data)
    {
        //캐릭터의 등급을 격하하는 경우,
        //격하 정도를 정의
        //0 = 격하하지 않음, 1 = 1단계 격하

        // 사형수 등급이 중급이나 하급이면서 3등급의 범죄를 저질렀을 경우
        // 단, 전과가 없을 경우 제외
        if (((int.Parse(data["positionGrade"]) == 2 ||
            int.Parse(data["positionGrade"]) == 1) &&
            int.Parse(data["crimeGrade"]) == 2) &&
            int.Parse(data["crimeRecord"]) != 0)
        {
            data["degradation"] = "1";
        }
        // 사형수 등급이 상급이고, 2등급의 범죄를 저질렀으며
        // 피해자가 상급 이상, 2급 이상의 전과가 있을 때 격하
        // 단, 사형수 직업이 판사, 검사, 정치인인 경우 제외
        else if ((int.Parse(data["positionGrade"]) == 3 &&
            int.Parse(data["crimeGrade"]) == 1) &&
            int.Parse(data["victimGrade"]) >= 3 &&
            int.Parse(data["crimeRecord"]) >= 2 &&
            int.Parse(data["job"]) != 1) //이 부분 모르겠음
        {
            data["degradation"] = "1";
        }
        // 사형수 등급이 상급이고, 3등급의 범죄를 저질렀으며
        // 3급 이상의 전과가 있을 때 격하
        // 단, 사형수 직업이 특정직업인 경우 제외
        else if ((int.Parse(data["positionGrade"]) == 3 &&
            int.Parse(data["crimeGrade"]) == 3) &&
            int.Parse(data["crimeRecord"]) >= 3 &&
            int.Parse(data["job"]) != 1) //이 부분 모르겠음
        {
            data["degradation"] = "1"; 
        }
        // 사형수 등급이 중급이고, 2등급 이상의 범죄를 저질렀으며
        // 2급 이상의 전과가 있을 때 격하
        else if ((int.Parse(data["positionGrade"]) == 2 &&
            int.Parse(data["crimeGrade"]) <= 1) &&
            int.Parse(data["crimeRecord"]) >= 2)
        {
            data["degradation"] = "1";
        }
        // 사형수 등급이 중급~하급이고 시민 등급 변경 사유가 모범이며
        // 전과가 있을 경우 무조건 격하
        else if ((int.Parse(data["positionGrade"]) <= 2 &&
            int.Parse(data["brandChange"]) == 1 &&
            int.Parse(data["crimeRecord"]) >= 1))
        {
            data["degradation"] = "1";
        }
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
        {
            if (isApplySpecificInfo && readPrisonerInfo.GetAttackerJob() != -1)
                specialJobFlag = readPrisonerInfo.GetAttackerJob();
            else
                specialJobFlag = getRandomValueByRange(probabilityInfo["attackerSpecialJob"][int.Parse(data["positionGrade"])]);
        }
        else
        {
            if (isApplySpecificInfo && readPrisonerInfo.GetVictimJob() != -1)
                specialJobFlag = readPrisonerInfo.GetVictimJob();
            else
                specialJobFlag = getRandomValueByRange(probabilityInfo["victimSpecialJob"][0]);
        }

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
            //유민 수정
            if (isApplySpecificInfo && readPrisonerInfo.GetCrimeRecord() != -1)
            {
                data["crimeRecord"] = readPrisonerInfo.GetCrimeReason().ToString();
            }
            else if(data["positionGrade"].Equals("0"))
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

                //유민 추가
                if (isApplySpecificInfo && readPrisonerInfo.GetLie() != -1)
                    lieORInfoErrorDistinguish = readPrisonerInfo.GetLie();

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