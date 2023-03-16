using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerInfo : MonoBehaviour
{
    public RecordData recordData;
    [SerializeField] TableManager tableManager;
    [SerializeField] Ask ask;
    [SerializeField] Lie lie;

    void Start()
    {
        recordData = new RecordData(tableManager);

        if (recordData.attackerData.ContainsKey("ask") && recordData.attackerData["ask"].Equals("1")) ActiveAsk();
        //if (recordData.attackerData.ContainsKey("lie") && 
    }

    void ActiveAsk()
    {
        ask.ActiveAsk();
    }

    void 
}