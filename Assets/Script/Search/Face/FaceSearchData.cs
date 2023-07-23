using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSearchData : MonoBehaviour
{
    [SerializeField] private int faceShape;
    [SerializeField] private int eyes;
    [SerializeField] private int nose;
    [SerializeField] private int ears;
    [SerializeField] private int dimples;
    [SerializeField] private int mouth;
    

    private void Awake()
    {
        // 넙데데형 : 0 | 투박형 : 1 | 계란형 : 2 | 긴얼굴형 : 3
        faceShape = Random.Range(0, 4);

        // 유쌍 : 0 | 무쌍 : 1
        eyes = Random.Range(0, 2);

        // 메부리코 : 0 | 일반코 : 1
        nose = Random.Range(0, 2);

        // 얇은입술 : 0 | 두꺼운입술 : 1
        mouth = Random.Range(0, 2);

        // 연결된 귓볼 : 0 | 연결X 귓볼 : 1
        ears = Random.Range(0, 2);

        // 보조개X : 0 | 보조개O : 1
        mouth = Random.Range(0, 2);
    }

    public int GetFaceShape()   {   return faceShape;   }
    public int GetEyes()    {   return eyes;    }
    public int GetNose()    {   return nose;    }
    public int GetEars()    {   return ears;    }
    public int GetDimples()     {   return dimples;     }
    public int GetMouth()   {   return mouth;   }
}
