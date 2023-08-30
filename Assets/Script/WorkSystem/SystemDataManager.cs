using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemDataManager : MonoBehaviour
{
    //유전자 코드, 몸수색, 얼굴수색 시스템 등의 데이터 관리

    public bool isClone;

    private void Awake()
    {
        SetClone();
    }

    private void SetClone()
    {
        System.Random random = new System.Random();

        if (random.Next(0, 3) == 1)
        {
            isClone = true;
        }
        else
        {
            isClone = false;
        }
    }
}

