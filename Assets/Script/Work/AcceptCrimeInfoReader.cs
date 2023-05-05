using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class AcceptCrimeInfoReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    private Dictionary<int, List<int>> _accpetCrimeDictionary = new Dictionary<int, List<int>>();

    public AcceptCrimeInfoReader(TextAsset data)
    {
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) 
            return;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values[0] == "")
                continue;

            int day = 0;
            List<int> acceptCrimeList = new List<int>();
            for (var j = 1; j < header.Length; j++)
            {
                day = int.Parse(header[j]);
                string value = values[j].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                List<string> valueList = value.Split(';').ToList();

                foreach (string valueString in valueList)
                    acceptCrimeList.Add(int.Parse(valueString));
            }
            _accpetCrimeDictionary.Add(day, acceptCrimeList);
        }
    }

    public List<int> getAccpetCrimeDictionary(int day)
    {
        if (_accpetCrimeDictionary.ContainsKey(day) == false)
            return null;

        return _accpetCrimeDictionary[day];
    }
}
