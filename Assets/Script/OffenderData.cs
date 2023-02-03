using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffenderData
{
    public string name { get; private set; }
    public string fname { get; private set; }
    public string vname { get; private set; }
    public string vfname { get; private set; }
    public string crime { get; private set; }
    public string detail { get; private set; }
    public string grade { get; private set; }
    public int isHanging { get; private set; }

    public OffenderData()
    {
        Dictionary<string, string> data = TableManager.GetData();

        name = data["name"];
        fname = data["fname"]; 
        vname = data["vname"];
        vfname = data["vfname"];
        crime = data["crime"];
        detail = data["crimeReason"];
        grade = data["grade"];

        //사형 판별//
        List<List<Dictionary<string, List<string>>>> judgeList = TableManager.judgeT;
        int day = HangingManager.day;
        bool f = false;

        Debug.Log(data["familyGrade"]);
        Debug.Log(data["positionGrade"]);
        Debug.Log(data["crimeGrade"]);

        for (int i = day - 1; i >= 0; i--)
        {
            if (f) break;
            for (int j = 0; j < judgeList[i].Count; j++)
            {
                List<string> headerList = judgeList[i][0]["header"];
                bool isMatch = true;
                for (int k = 0; k < headerList.Count - 1; k++)
                {
                    string header = headerList[k];
                    bool subMatch = false;
                    foreach (string str in judgeList[i][j][header])
                    {
                        if (str.Equals(data[header]))
                        {
                            subMatch = true;
                            break;
                        }
                    }

                    if (!subMatch) isMatch = false;
                }

                if (isMatch)
                {
                    f = true;
                    isHanging = int.Parse(judgeList[i][j]["judgement"][0]);
                    Debug.Log(j);
                    Debug.Log(isHanging);
                    break;
                }
            }
        }
    }
}