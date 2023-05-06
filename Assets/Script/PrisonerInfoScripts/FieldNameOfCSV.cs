using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FieldNameOfCSV : MonoBehaviour
{
    public List<string> fieldNames; //Prisoner_dayN 의 각 필드명 리스트

    [SerializeField]
    string fileName;
    int day = HangingManager.day;

    ReadPrisonerInfo readPrisonerInfo;  //임시

    private void Awake()
    {
        readPrisonerInfo = GetComponent<ReadPrisonerInfo>();    //임시
        day = readPrisonerInfo.day; //임시

        char charValue = (char)(day + '0');

        fileName = "Prisoner_day";
        fileName += charValue;

        fieldNames = new List<string>();

        using (var reader = new StreamReader("Assets/Resources/" + fileName + ".csv"))
        {
            string line = reader.ReadLine();
            if (line != null)
            {
                string[] fields = line.Split(',');
                foreach (string field in fields)
                {
                    fieldNames.Add(field);
                }
            }
        }

        //Debug.Log("Field Names: " + string.Join(", ", fieldNames.ToArray()));
    }

    void Start()
    {

    }
}
