using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class WorkProbabilityCSVRedaer
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public Dictionary<string, List<List<int>>> Read(string file)
    {
        var dictionary = new Dictionary<string, List<List<int>>>();
        TextAsset data = Resources.Load(file) as TextAsset;
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) 
            return dictionary;

        var header = Regex.Split(lines[0], SPLIT_RE);

        int lineLength = lines.Length;
        for (var i = 1; i < lineLength; ++i)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0)
                continue;

            int headerLength = header.Length;
            for (var j = 0; j < headerLength; j++)
            {
                if (values[j].Equals(""))
                    continue;

                if (dictionary.ContainsKey(header[j]) == false)
                    dictionary.Add(header[j], new List<List<int>>());

                string[] stringArray = values[j].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "").Split(';');
                List<int> valueList = new List<int>();

                foreach (string str in stringArray)
                {
                    if (int.TryParse(str, out int intValue))
                        valueList.Add(intValue);
                    else
                        Debug.LogWarning($"'{str}' is not a valid integer and has been skipped.");
                }

                dictionary[header[j]].Add(valueList);
            }
        }

        return dictionary;
    }
}