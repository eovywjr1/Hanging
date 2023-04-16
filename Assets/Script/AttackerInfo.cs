using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerInfo : MonoBehaviour
{
    public RecordData recordData;
    TableManager tableManager;
    [SerializeField] Lie lie;

    private void Awake()
    {
        tableManager = FindObjectOfType<TableManager>();
    }

    void Start()
    {
        recordData = new RecordData(tableManager);

        EventManager.instance.postNotification("activeAsk", this, null); //임시 (버튼 누를 때 이거 실행)
        //if (recordData.attackerData.ContainsKey("ask") && recordData.attackerData["ask"].Equals("1"))
        //    EventManager.instance.postNotification("activeAsk", this, null);
    }

    public RecordData GetRecordData()
    {
        return recordData;
    }
}