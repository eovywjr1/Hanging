using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerInfo : MonoBehaviour
{
    public RecordData recordData;
    TableManager tableManager;
    ReadPrisonerInfo readPrisonerInfo;
    [SerializeField] Ask ask;
    [SerializeField] Lie lie;

    private void Awake()
    {
        tableManager = FindObjectOfType<TableManager>();
        readPrisonerInfo = FindObjectOfType<ReadPrisonerInfo>();
    }

    void Start()
    {
        recordData = new RecordData(tableManager, readPrisonerInfo);
    }

    public RecordData GetRecordData()
    {
        return recordData;
    }
}