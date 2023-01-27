using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    private static List<List<object>> nameT, fnameT, crimeT, detailT;
    private List<List<object>> judgeT;
    public static List<int[,,]> judgeList = new List<int[, , ]>();

    void Awake()
    {
        if (nameT == null)
        {
            nameT = CSVReader.Read("사건기록서 이름");
            fnameT = CSVReader.Read("사건기록서 성");
            crimeT = CSVReader.Read("사건기록서 죄명");
            detailT = CSVReader.Read("사건기록서 경위");
            judgeT = CSVReader.Read("사건기록서 판단"); GetJudgeTable();
        }
    }

    public static Dictionary<string,string> GetData()
    {
        Dictionary<string,string> data = new Dictionary<string,string>();
        
        data["name"] = GetString(nameT);
        data["fname"] = Getfname(data);
        data["crime"] = Getcrime(data);
        data["detail"] = Getdetail(data, data["crime"]);
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

    private static string Getcrime(Dictionary<string, string> data)
    {
        int headerid = Random.Range(0, crimeT.Count);
        List<string> temp = new List<string>();
        foreach(var item in crimeT[headerid])
        {
            if (!item.ToString().Equals(""))
                temp.Add(item.ToString());
            else
                break;
        }
        int valueid = Random.Range(0, temp.Count);

        data["crimeGrade"] = headerid.ToString();
        data["cgrade"] = data["crimeGrade"];

        return temp[valueid];
    }

    private static string Getfname(Dictionary<string, string> data)
    {
        int headerid = Random.Range(0, fnameT.Count);
        int valueid = Random.Range(0, fnameT[headerid].Count);

        data["grade"] = GetGrade(headerid);
        data["fgrade"] = (6 - headerid).ToString();
        data["sgrade"] = GetCGrade(headerid);

        return (string)fnameT[headerid][valueid];
    }

    private static string GetString(List<List<object>> list)
    {
        int headerid = Random.Range(0, list.Count);
        int valueid = Random.Range(0, list[headerid].Count);

        return (string)list[headerid][valueid];
    }

    private static string Getdetail(Dictionary<string, string> data, string crime)
    {
        string grade = data["crimeGrade"];

        List<string> randomlist = new List<string>();
        for(int i = 0; i < detailT[0].Count; i++)
        {
            if (detailT[1][i].Equals("ALL"))
                randomlist.Add((string)detailT[0][i]);
            else
            {
                string str = detailT[1][i].ToString();
                foreach (var v in str)
                {
                    if (grade.Equals(v))
                    {
                        randomlist.Add((string) detailT[0][i]);
                        break;
                    }
                }
            }
        }
        int valueid = Random.Range(0,randomlist.Count);

        return randomlist[valueid];
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
    private static string GetCGrade(int grade)
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

    private void GetJudgeTable()
    {
        judgeList.Add(new int[judgeT[0].Count, judgeT[0].Count, judgeT[0].Count]);

        for (int i = 0; i < judgeT[0].Count; i++)
        {
            List<string> fgrade = (judgeT[0][i] as string).Split(',').ToList();
            List<string> sgrade = (judgeT[1][i] as string).Split(',').ToList();
            List<string> cgrade = (judgeT[2][i] as string).Split(',').ToList();

            for (int q = 0; q < fgrade.Count; q++)
            {
                for (int w = 0; w < sgrade.Count; w++)
                {
                    for (int e = 0; e < cgrade.Count; e++) judgeList[0][int.Parse(fgrade[q]), int.Parse(sgrade[w]), int.Parse(cgrade[e])] = int.Parse(judgeT[3][i] as string);
                }
            }
        }

        Debug.Log(judgeList[0][2,1,3]);
    }
}
