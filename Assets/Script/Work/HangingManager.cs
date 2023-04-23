using Kino;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HangingManager : MonoBehaviour, IListener
{
    public AttackerMouseMove attackerMouseMove;
    public AttackerInfo attackerInfo;
    private AnalogGlitch analogGlitch;
    [SerializeField] BossHand bossHand;
    HangingTimer hangingTimer;
    public DialogWindowController dialogWindowController;
    [SerializeField] GameObject convertEffect, attackerPrefab;

    public bool isTodesstrafe, isAmnesty, isActiveAsk;
    public static bool isCompulsoryEnd;
    int _attackerCount = 1;
    public int attackerCount
    {
        get { return _attackerCount; }
        set { 
            _attackerCount = value;
            EventManager.instance.postNotification("updateAttackerCountCCTV", this, _attackerCount);
        }
    }
            
    public static int day = 1;
    static bool isCorrect = true;

    private void Awake()
    {
        dialogWindowController = FindObjectOfType<DialogWindowController>();
        hangingTimer = FindObjectOfType<HangingTimer>();
        analogGlitch = FindObjectOfType<AnalogGlitch>();
        InitialAttacker();
    }

    private void Start()
    {
        if (!isCorrect)
        {
            StartCoroutine(StartHoldOutHands());
            isCorrect = true;
        }

        EventManager.instance.addListener("amnesty", this);
        EventManager.instance.addListener("todesstrafe", this);
        EventManager.instance.addListener("activeAsk", this);
    }

    public void EndTodesstrafe()
    {
        DestroyAllLineAndWindow();
        NextAttacker();

        if(isActiveAsk && attackerInfo.recordData.attackerData["ask"].Equals("1") == false)
        {
            if (Ask.isFirst)
            {
                Ask.isFirst = false;
                EventManager.instance.postNotification("dialogEvent", this, 63);
            }
            else
            {
                EventManager.instance.postNotification("dialogEvent", this, UnityEngine.Random.Range(64, 71));
            }
        }
    }

    public void Todesstrafe()
    {
        isTodesstrafe = true;

        //���� �Ǻ�//
        if (DistinguishTodesstrafe(0)) Debug.Log("True");
        else Debug.Log("False");
        Debug.Log("����");

        EndTodesstrafe();
    }

    public void Amnesty()
    {
        isAmnesty = true;

        //���� �Ǻ�//
        if (DistinguishTodesstrafe(1)) Debug.Log("True");
        else Debug.Log("False");
        Debug.Log("����");

        EndTodesstrafe();
    }

    private bool DistinguishTodesstrafe(int mode)
    {
        if (isCompulsoryEnd == false || mode == attackerInfo.recordData.isHanging)
        {
            Debug.Log(attackerInfo.recordData.isHanging);
            isCorrect = false;

            return true;
        }
        else
        {
            Debug.Log(attackerInfo.recordData.isHanging);
            isCorrect = false;
            StartCoroutine(StartStateWrong());

            return false;
        }
    }

    private void DestroyAllLineAndWindow()
    {
        attackerMouseMove.StopAllCoroutines();
        foreach (Line line in Line.lineList)
        {
            Destroy(line.lineObject);
            Destroy(line.windowObject);
        }
        Line.lineList.Clear();
    }

    void NextAttacker()
    {
        _attackerCount++;

        StartCoroutine(ConvertAttackerEffect());
    }

    IEnumerator ConvertAttackerEffect()
    {
        convertEffect.SetActive(true);

        yield return new WaitForSecondsRealtime(1.5f);

        convertEffect.SetActive(false);
        InitialAttacker();
    }

    IEnumerator StartGlitch()
    {
        analogGlitch._isGlitch = true;
        yield return new WaitForSecondsRealtime(0.75f);
        analogGlitch._isGlitch = false;

        NextAttacker();
    }


    IEnumerator StartHoldOutHands()
    {
        yield return new WaitForSecondsRealtime(1.25f);
        EventManager.instance.postNotification("badge", this, null);
        EventManager.instance.postNotification("dialogEvent", this, UnityEngine.Random.Range(53, 58));
    }

    public IEnumerator StartStateWrong() //������ Ŭ�� Ʋ���� 
    {
        analogGlitch._isGlitch = true;
        yield return new WaitForSecondsRealtime(0.75f);
        analogGlitch._isGlitch = false;

        StartCoroutine(StartHoldOutHands());
    }

    public void EndCompulsory()
    {
        isCompulsoryEnd = true;

        EventManager.instance.postNotification("dialogEvent", this, "createAttacker");

        hangingTimer.SetTimer(true);
        EventManager.instance.postNotification("moveCameraToDesk", this, null);
        EventManager.instance.postNotification("todesstrafe", this, null);
        dialogWindowController.SetEnabled(true);

    }

    void InitialAttacker()
    {
        if (attackerMouseMove != null)
            Destroy(attackerMouseMove.gameObject);

        GameObject attacker = Instantiate(attackerPrefab);
        attackerMouseMove = attacker.GetComponent<AttackerMouseMove>();
        attackerInfo = attacker.GetComponent<AttackerInfo>();

        if (isCompulsoryEnd)
        {
            EventManager.instance.postNotification("todesstrafe", this, null);
            EventManager.instance.postNotification("amnesty", this, null);
        }

        isTodesstrafe = false;
        isAmnesty = false;
        isActiveAsk = false;
    }

    public void OnEvent(string eventType, Component sender, object parameter = null)
    {
        switch (eventType)
        {
            case "amnesty":
                Amnesty();
                break;
<<<<<<< HEAD
=======

            case "todesstrafe":
                Todesstrafe();
                break;

            case "activeAsk":
                isActiveAsk = true;
                break;
>>>>>>> MinsuDelveop
        }
    }
}