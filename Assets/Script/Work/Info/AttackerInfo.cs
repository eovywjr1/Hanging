using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerInfo : MonoBehaviour
{
    public RecordData recordData;
    TableManager tableManager;
    ReadPrisonerInfo readPrisonerInfo;
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

    public bool checkAttackerPossibleAsk() 
    {
        if (recordData.attackerData.ContainsKey("ask") == false)
            return false;

        if (recordData.attackerData["ask"].Equals("-1"))
            return false;

        return true;
    }

    public bool checkAttackerAcceptAsk()
    {
        if (recordData.attackerData.ContainsKey("askAccept") == false)
            return false;

        if (recordData.attackerData["askAccept"].Equals("1") == false)
            return false;

        return true;
    }

    public bool checkAttackerReplyAsk() 
    {
        if (recordData.attackerData.ContainsKey("askReply") == false)
            return false;

        if (recordData.attackerData["askReply"].Equals("1") == false)
            return false;

        return true;
    }
}