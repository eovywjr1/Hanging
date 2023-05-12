using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingInfoWrapper
{
    public int _day = 0;
    public int _judgeCount = 0;
    public int _correctJudgeCount = 0;
    public int _discorrectJudgeCount = 0;
    public int _discorrectAndTodesstrafedPersonCount = 0;

    public HangingInfoWrapper(int day = 0, int judgeCount = 0, int correctJudgeCount = 0, int discorrectJudgeCount = 0, int discorrectAndTodesstrafedPersonCount = 0)
    {
        _day = day;
        _judgeCount = judgeCount;
        _correctJudgeCount = correctJudgeCount;
        _discorrectJudgeCount = discorrectJudgeCount;
        _discorrectAndTodesstrafedPersonCount = discorrectAndTodesstrafedPersonCount;
    }
}
