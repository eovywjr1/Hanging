using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public List<Dictionary<string, List<string>>> Read(string file)
    {
        var list = new List<Dictionary<string, List<string>>>();
        TextAsset data = Resources.Load(file) as TextAsset;
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);
        
        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            list.Add(new Dictionary<string, List<string>>());

            for (var j = 0; j < header.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");

                List<string> valueList = value.Split(',').ToList();
                list[i - 1].Add(header[j], valueList);
            }
        }

        //헤더(조건)를 0번째 Dictionary에 모두 저장//
        List<string> headerList = new List<string>();
        for (int j = 0; j < header.Length; j++) headerList.Add(header[j]);
        list[0].Add("header", headerList);

        return list;
    }
}