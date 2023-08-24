using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeneticCode : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    const string cloneCode = "ticlo";  //임시 복제인간 코드

    private string geneticCode;
    public bool isClone;

    private void Awake()
    {
        SetClone();
        GenerateRandomCode();

        text.text = geneticCode;
        Debug.Log("랜덤 유전자 코드는 " + geneticCode);
        Debug.Log("랜덤 유전자 코드의 길이는 " + geneticCode.Length);
    }

    private void SetClone()
    {
        System.Random random = new System.Random();

        if(random.Next(0,3) == 1)  
        {
            isClone = true;
        }
        else
        {
            isClone = false;
        }
    }

    private void GenerateRandomCode()
    {
        System.Random random = new System.Random();

        const string alphabet = "abcdefghijklmnopqrstuvwxyz";
        int stringPerGroup = 3;
        int numberOfGroups = 5;
        int numberOfLines = 3;
        int groupForGene = -1;
        int nowGroupIdx = 0;

        if(isClone)
        {
            groupForGene = random.Next(0, 9);
        }

        for(int i=0;i<numberOfLines;i++)
        { 
            for(int j=0;j<stringPerGroup;j++)
            {
                if(nowGroupIdx == groupForGene)
                {
                    geneticCode += cloneCode;
                }
                else
                {
                    for (int k = 0; k < numberOfGroups; k++)
                    {
                        geneticCode += alphabet[random.Next(alphabet.Length)];
                    }
                }
                geneticCode += " ";

                nowGroupIdx++;
            }
            geneticCode += "\n";
        }
    }
}
