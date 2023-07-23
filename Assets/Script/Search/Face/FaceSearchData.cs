using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSearchData : MonoBehaviour
{
    public Dictionary<string, int> faceData;

    private void Awake()
    {
        faceData = new Dictionary<string, int>();

        // 최상급 : 1 | 상급 : 2 | 최하급 : 3 | 어떤 등급 조건도 충족X : 0 
        faceData["faceGrade"] = Random.Range(0, 4);

        // 넙데데형 : 0 | 투박형 : 1 | 계란형 : 2 | 긴얼굴형 : 3
        // 유쌍 : 0 | 무쌍 : 1
        // 메부리코 : 0 | 일반코 : 1
        // 얇은입술 : 0 | 두꺼운입술 : 1
        // 연결된 귓볼 : 0 | 연결X 귓볼 : 1
        // 보조개X : 0 | 보조개O : 1
        // 직모: 0 | 곱슬모 : 1

        switch(faceData["faceGrade"])
        {
            case 1:
                SetHighClass();
                break;
            case 2:
                SetMiddleClass();
                break;
            case 3:
                SetLowClass();
                break;
            case 0:
                SetNoneClass();
                break;
            default:
                Debug.Log("얼굴등급설정X");
                break;
        }
    }

    void SetHighClass()
    {
        faceData["faceShape"] = Random.Range(1, 3); //투박한 얼굴형 : 1 || 계란형 : 2
        faceData["eyes"] = 1;       //무쌍
        faceData["nose"] = 1;       //일반코(메부리X)
        faceData["mouth"] = 0;      //얇은 입술
        faceData["ears"] = 0;       //연결된 귓볼
        faceData["dimples"] = 0;    //보조개 없음
        faceData["hairStyle"] = 0;  //직모
    }

    void SetMiddleClass()
    {
        faceData["faceShape"] = Random.Range(0, 3); //넙데데 얼굴형 : 0 || 투박한 얼굴형 : 1 || 계란형 : 2
        faceData["eyes"] = 1;                   //무쌍
        faceData["nose"] = Random.Range(0,2);   //메부리코 : 0 || 일반코 : 1
        faceData["mouth"] = Random.Range(0,2);  // 얇은입술 : 0 | 두꺼운입술 : 1
        faceData["ears"] = 0;                   //연결된 귓볼
        faceData["dimples"] = Random.Range(0,2);//보조개X: 0 | 보조개O : 1
        faceData["hairStyle"] = 0;  //직모
    }

    void SetLowClass()
    {
        faceData["faceShape"] = Random.Range(0, 3); //넙데데 얼굴형 : 0 || 투박한 얼굴형 : 1 || 계란형 : 2
        faceData["eyes"] = 0;       //유쌍: 0
        faceData["nose"] = 0;       //메부리코 : 0
        faceData["mouth"] = 0;      //얇은입술 : 0
        faceData["ears"] = 1;       //연결X 귓볼 : 1
        faceData["dimples"] = 1;    //보조개O : 1
        faceData["hairStyle"] = 1;  //곱슬모
    }

    void SetNoneClass()
    {
        // 넙데데형 : 0 | 투박형 : 1 | 계란형 : 2 | 긴얼굴형 : 3
        faceData["faceShape"] = Random.Range(0, 3);
        // 유쌍 : 0 | 무쌍 : 1
        faceData["eyes"] = Random.Range(0, 2);
        // 메부리코 : 0 | 일반코 : 1
        faceData["nose"] = Random.Range(0, 2);
        // 얇은입술 : 0 | 두꺼운입술 : 1
        faceData["mouth"] = Random.Range(0, 2);
        // 연결된 귓볼 : 0 | 연결X 귓볼 : 1
        faceData["ears"] = Random.Range(0, 2);
        // 보조개X : 0 | 보조개O : 1
        faceData["dimples"] = Random.Range(0, 2);
        // 직모: 0 | 곱슬모 : 1
        faceData["dimples"] = Random.Range(0, 2);
    }
}