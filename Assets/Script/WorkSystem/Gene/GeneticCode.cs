using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeneticCode : WorkSystemData
{
    [SerializeField] TMP_Text text;

    const string cloneCode = "ticlo";  //임시 복제인간 코드 (임의로 정함)

    private string geneticCode;

    private void Awake()
    {
        GenerateRandomCode();

        text.text = geneticCode;
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
