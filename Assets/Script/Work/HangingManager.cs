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
    public static AttackerDialogEvent attackerDialogEvent;
    private AnalogGlitch analogGlitch;
    [SerializeField] BossHand bossHand;
    HangingTimer hangingTimer;
    public DialogWindowController dialogWindowController;
    [SerializeField] GameObject convertEffect, attackerPrefab;

    public bool isTodesstrafe, isAmnesty;
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
    }

    public void EndTodesstrafe()
    {
        DestroyAllLineAndWindow();
        NextAttacker();
    }

    public void Todesstrafe()
    {
        isTodesstrafe = true;

        //사형 판별//
        if (DistinguishTodesstrafe(0)) Debug.Log("True");
        else Debug.Log("False");
        Debug.Log("사형");

        EndTodesstrafe();
    }

    public void Amnesty()
    {
        isAmnesty = true;

        //사형 판별//
        if (DistinguishTodesstrafe(1)) Debug.Log("True");
        else Debug.Log("False");
        Debug.Log("생존");

        EndTodesstrafe();
    }

    private bool DistinguishTodesstrafe(int mode)
    {
        if (mode == attackerInfo.recordData.isHanging)
        {
            Debug.Log(attackerInfo.recordData.isHanging);
            isCorrect = false;

            return true;
        }
        else
        {
            Debug.Log(attackerInfo.recordData.isHanging);
            isCorrect = false;
            StartCoroutine(StartGlitch());

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

        bossHand.holdOutHand();
    }

    public IEnumerator StartStateWrong() //진술서 클릭 틀릴때 
    {
        analogGlitch._isGlitch = true;

        yield return new WaitForSecondsRealtime(0.75f);

        analogGlitch._isGlitch = false;

        yield return new WaitForSecondsRealtime(1.25f);

        bossHand.holdOutHand();
    }

    public void EndCompulsory()
    {
        isCompulsoryEnd = true;

        attackerDialogEvent.SetSituationDialogEvent(UnityEngine.Random.Range(1, 11), 3f);

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
        attackerDialogEvent = attacker.GetComponent<AttackerDialogEvent>();

        if (isCompulsoryEnd)
        {
            EventManager.instance.postNotification("todesstrafe", this, null);
            EventManager.instance.postNotification("amnesty", this, null);
        }

        isTodesstrafe = false;
        isAmnesty = false;
    }

    public void OnEvent(string eventType, Component sender, object parameter = null)
    {
    }
}