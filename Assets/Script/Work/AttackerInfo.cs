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
    }

    public RecordData GetRecordData()
    {
        return recordData;
    }
}