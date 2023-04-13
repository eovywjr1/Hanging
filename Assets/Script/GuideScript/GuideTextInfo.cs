using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideTextInfo : MonoBehaviour
{
    public int day;         //일차
    public string type;     //type: 목차, 소/소소목차, 제목, 내용
    public string contents; //가이드창에 들어갈 텍스트
    public string number;   //목차와 제목 연결
    public string note;     //기타 텍스트 디자인 특이사항 
}
