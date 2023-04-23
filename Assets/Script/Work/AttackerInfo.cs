using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerInfo : MonoBehaviour
{
    public RecordData recordData;
    TableManager tableManager;
    [SerializeField] Ask ask;
    [SerializeField] Lie lie;

    private void Awake()
    {
        tableManager = FindObjectOfType<TableManager>();
    }

    void Start()
    {
        recordData = new RecordData(tableManager);
<<<<<<< HEAD:Assets/Script/AttackerInfo.cs

        if (recordData.attackerData.ContainsKey("ask") && recordData.attackerData["ask"].Equals("1")) ActiveAsk();
    }

    void ActiveAsk()
    {
        ask.ActiveAsk();
=======
>>>>>>> MinsuDelveop:Assets/Script/Work/AttackerInfo.cs
    }

    public RecordData GetRecordData()
    {
        return recordData;
    }
}