using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class SpecialJobIndexInfoReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    //int형의 -1 값은 all을 의미함//
    private List<int> _dayList = new List<int>();
    private List<string> _jobNameList = new List<string>();
    private List<int> _positionGradeList = new List<int>();
    private List<int> _isAttackerList = new List<int>();
    private List<int> _specialJobIndexList = new List<int>();

    public SpecialJobIndexInfoReader(TextAsset data)
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

                if (header[j].Equals("day"))
                    _dayList.Add(int.Parse(values[i]));
                else if (header[j].Equals("job"))
                    _jobNameList.Add(values[j]);
                else if (header[j].Equals("positionGrade"))
                    _positionGradeList.Add(int.Parse(values[i]));
                else if (header[j].Equals("isAttacker"))
                    _isAttackerList.Add(int.Parse(values[i]));
                else if (header[j].Equals("index"))
                    _specialJobIndexList.Add(int.Parse(values[i]));
            }
        }
    }

    public int getSpecialJobIndex(int day, string job, int positionGrade, int isAttacker)
    {
        int listCount = _dayList.Count;
        for(int index = 0; index < listCount; ++index)
        {
            if ((day == _dayList[index]) && (job.Equals(_jobNameList[index])) && ((_positionGradeList[index] == -1) || (positionGrade == _positionGradeList[index])) && ((_isAttackerList[index] == -1) || (isAttacker == _isAttackerList[index])))
                return _specialJobIndexList[index];
        }

        return 0;
    }
}