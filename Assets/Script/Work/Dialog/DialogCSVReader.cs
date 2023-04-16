using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class DialogCSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public Dictionary<string, List<List<string>>> Read(string file)
    {
        var list = new Dictionary<string, List<List<string>>>();
        TextAsset data = Resources.Load(file) as TextAsset;
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            if(list.ContainsKey(values[0]) == false)
                list.Add(values[0], new List<List<string>>());

            List<string> valueList = new List<string>();
            for (var j = 1; j < header.Length; j++)
                valueList.Add(values[j]);
            list[values[0]].Add(valueList);
        }

        return list;
    }
}
