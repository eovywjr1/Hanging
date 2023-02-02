using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    private static List<Dictionary<string, List<string>>> nameT, fnameT, crimeT, detailT;
    public static List<List<Dictionary<string, List<string>>>> judgeT = new List<List<Dictionary<string, List<string>>>>();

    void Awake()
    {
        if (nameT == null)
        {
            nameT = CSVReader.Read("사건기록서 이름");
            Debug.Log("1");
            fnameT = CSVReader.Read("사건기록서 성");
            crimeT = CSVReader.Read("사건기록서 죄명");
            detailT = CSVReader.Read("사건기록서 경위");

            string fileName = "사건기록서 판단";
            fileName += "1"; // day 더할 예정
            judgeT.Add(CSVReader.Read(fileName));
        }
    }

    public static Dictionary<string,string> GetData()
    {
        Dictionary<string,string> data = new Dictionary<string,string>();
        
        data["name"] = GetString(nameT);
        Getfname(data);
        Getcrime(data);
        Getdetail(data, data["crime"]);

        data["vfname"] = GetString(fnameT);
        data["vname"] = GetString(nameT);
        if (data["vfname"].Equals(data["fname"]))
        {
            while (!data["vname"].Equals(data["name"]))
            {
                data["vname"] = GetString(nameT);
            }
        }

        return data;
    }

    private static void Getcrime(Dictionary<string, string> data)
    {
        while (true)
        {
            int valueid = Random.Range(0, crimeT.Count);
            int headerid = Random.Range(0, crimeT[0]["header"].Count);

            string str = crimeT[valueid][crimeT[0]["header"][headerid]][0];
            if (!str.Equals("")){
                data["crime"] = str;
                data["crimeGrade"] = crimeT[0]["header"][headerid][0].ToString();

                return;
            }
        }
    }

    private static void Getfname(Dictionary<string, string> data)
    {
        int valueid = Random.Range(0, fnameT.Count);
        int headerid = Random.Range(0, fnameT[0]["header"].Count);

        data["grade"] = GetGrade(headerid);
        data["familyGrade"] = (6 - headerid).ToString();
        data["positionGrade"] = GetPositionGrade(headerid);
        data["fname"] = fnameT[valueid][fnameT[0]["header"][headerid]][0];
    }

    private static string GetString(List<Dictionary<string, List<string>>> list)
    {
        int valueid = Random.Range(0, list.Count);
        int headerid = Random.Range(0, list[0]["header"].Count);

        return list[valueid][list[0]["header"][headerid]][0];
    }

    private static void Getdetail(Dictionary<string, string> data, string crime)
    {
        string grade = data["crimeGrade"];

        List<string> randomlist = new List<string>();
        for (int i = 0; i < detailT.Count; i++)
        {
            string applyGrade = detailT[i][detailT[0]["header"][1]][0];
            string detail = detailT[i][detailT[0]["header"][0]][0];

            if (applyGrade.Equals("ALL")) randomlist.Add(detail);
            else
            {
                foreach (var v in detailT[i][detailT[0]["header"][1]])
                {
                    if (grade.Equals(v))
                    {
                        randomlist.Add(detail);
                        break;
                    }
                }
            }
        }

        int valueid = Random.Range(0,randomlist.Count);
        data["detail"] = randomlist[valueid];
    }

    private static string GetGrade(int grade)
    {
        switch (grade)
        {
            case 0:
                return "A";
            case 1:
                return "B";
            case 2:
                return "C";
            case 3:
                return "D";
            case 4:
                return "E";
            case 5:
                return "F";
            case 6:
                return "G";
            default:
                return null;
        }
    }

    //신분 등급//
    private static string GetPositionGrade(int grade)
    {
        switch (grade)
        {
            case 0:
                return "4";
            case 1:
                return "3";
            case 2:
            case 3:
                return "2";
            case 4:
            case 5:
                return "1";
            case 6:
                return "0";
            default:
                return null;
        }
    }
}
