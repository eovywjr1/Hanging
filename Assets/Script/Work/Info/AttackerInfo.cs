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
    public ScrollViewController scrollViewController;

    private void Awake()
    {
        tableManager = FindObjectOfType<TableManager>();
        readPrisonerInfo = FindObjectOfType<ReadPrisonerInfo>();
    }

    void Start()
    {

        recordData = new RecordData(tableManager, readPrisonerInfo);

        scrollViewController = FindObjectOfType<ScrollViewController>();
        scrollViewController.MakeMentList();

/*        recordData = new RecordData(tableManager);
        Debug.Log("정보생성완료");
        scrollViewController = FindObjectOfType<ScrollViewController>();
        scrollViewController.MakeMentList();*/

    }

    public RecordData GetRecordData()
    {
        return recordData;
    }
}