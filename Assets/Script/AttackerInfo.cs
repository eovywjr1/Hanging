using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerInfo : MonoBehaviour
{
    public RecordData recordData;
    [SerializeField] TableManager tableManager;
    [SerializeField] Ask ask;

    void Start()
    {
        recordData = new RecordData(tableManager);

        if (recordData.attackerData.ContainsKey("ask") && recordData.attackerData["ask"].Equals("1")) ActiveAsk();
    }

    void ActiveAsk()
    {
        ask.ActiveAsk();
    }
}