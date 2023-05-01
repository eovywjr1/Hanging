using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class JobInfoReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public List<string> _jobNameList { get; } = new List<string>();
    public List<List<int>> _familyGradeList { get; } = new List<List<int>>();
    private List<List<int>> _specialDayList = new List<List<int>>();
    public Dictionary<int, List<string>> _specialJobDictionary { get; } = new Dictionary<int, List<string>>();
    public Dictionary<int, List<string>> _normalJobDictionary { get; } = new Dictionary<int, List<string>>();

    public JobInfoReader(TextAsset data)
    {
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1)
            return;

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

                if (header[j].Equals("jobName"))
                    _jobNameList.Add(values[j]);
                else
                {
                    string[] stringArray = values[j].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "").Split(';');

                    if (header[j].Equals("familyName"))
                    {
                        List<int> valueList = new List<int>();
                        foreach (string str in stringArray)
                        {
                            if (int.TryParse(str, out int intValue))
                                valueList.Add(intValue);
                            else
                                Debug.LogWarning($"'{str}' is not a valid integer and has been skipped.");
                        }
                        _familyGradeList.Add(valueList);
                    }
                    else if (header[j].Equals("specialDay"))
                    {
                        List<int> valueList = new List<int>();
                        foreach (string str in stringArray)
                        {
                            int specialDay = int.Parse(str);
                            valueList.Add(specialDay);

                            if (specialDay == 0)
                                break;

                            if (_specialJobDictionary.ContainsKey(specialDay) == false)
                                _specialJobDictionary.Add(specialDay, new List<string>());

                            _specialJobDictionary[specialDay].Add(values[0]);
                        }

                        _specialDayList.Add(valueList);
                    }
                }
            }
        }
    }

    public string getSpecialJob(int day)
    {
        return _specialJobDictionary[day][UnityEngine.Random.Range(0, _specialJobDictionary[day].Count)];
    }

    public string getNormalJob(int day)
    {
        if (_normalJobDictionary.ContainsKey(day))
            return _normalJobDictionary[day][UnityEngine.Random.Range(0, _normalJobDictionary[day].Count)];

        List<string> normalList = new List<string>();
        int index = 0;
        foreach(string str in _jobNameList)
        {
            bool isNormalJob = true;
            foreach(int specialDay in _specialDayList[index])
            {
                if (specialDay == day)
                    isNormalJob = false;
            }
            if (isNormalJob)
                normalList.Add(str);

            ++index;
        }
        _normalJobDictionary.Add(day, normalList);

        return _normalJobDictionary[day][UnityEngine.Random.Range(0, _normalJobDictionary[day].Count)];
    }

    public string getJobByAllList()
    {
        return _jobNameList[UnityEngine.Random.Range(0, _jobNameList.Count)];
    }
}