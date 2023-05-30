using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MakeGereticCode : MonoBehaviour
{
    TextMeshProUGUI code;

    private string humanCloneCode;
    private string geneticCode;

    const int numberOfCode = 9;
    const int lengthOfCode = 6;

    public bool isClone;

    private void Awake()
    {
        code = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        isClone = true; //임시로 복제인간이라 가정

        SetHumanCloneCode("abcdef");    //임시 복제인간 코드 넣음
        GenerateCode(isClone);

        code.text = geneticCode.ToString();
    }

    public void SetHumanCloneCode(string _humanCloneCode)
    {
        humanCloneCode = _humanCloneCode;
    }

    public void GenerateCode(bool isClone)
    {
        geneticCode = "";
        System.Random rand = new System.Random();

        //9개의 코드묶음 중 어디에 복제인간 코드를 넣을 것인지에 대한 인덱스
        int randomIdx = rand.Next(numberOfCode);

        for(int i=0;i<numberOfCode;i++)
        {
            if(isClone && randomIdx == i)
            {
                geneticCode += humanCloneCode;
            }
            else
            {
                for (int j = 0; j < lengthOfCode; j++)
                {
                    geneticCode += (char)(rand.Next(26) + 'a');
                }
            }
            geneticCode += " ";

            if ((i+1) % 3 == 0) geneticCode += "\n";
        }
    }
}
