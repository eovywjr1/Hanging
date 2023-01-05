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


    public OffenderData()
    {
        Dictionary<string, string> data = TableManager.GetData();

        name = data["name"];
        fname = data["fname"]; 
        vname = data["vname"];
        vfname = data["vfname"];
        crime = data["crime"];
        detail = data["detail"];
        grade = data["grade"];
    }
}
