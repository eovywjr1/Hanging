using Kino;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HangingManager : MonoBehaviour
{
    public AttackerMouseMove attackerMouseMove;
    public AttackerInfo attackerInfo;
    public static AttackerDialogEvent attackerDialogEvent;
    private AnalogGlitch analogGlitch;
    [SerializeField] BossHand bossHand;
    HangingTimer hangingTimer;
    public CameraMoveScript cameraMoveScript;
    public DialogWindowController dialogWindowController;
    [SerializeField] GameObject convertEffect, attackerPrefab;

    public bool isTodesstrafe, isAmnesty;
    public static bool isCompulsoryEnd;
    public static int day = 1, attackerCount = 1;
    static bool isCorrect = true;

    private void Awake()
    {
        dialogWindowController = FindObjectOfType<DialogWindowController>();
        cameraMoveScript = FindObjectOfType<CameraMoveScript>();
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
        attackerCount++;

        StartCoroutine(ConvertSceneEffect());
    }

    IEnumerator ConvertSceneEffect()
    {
        convertEffect.SetActive(true);

        yield return new WaitForSecondsRealtime(1.5f);

        convertEffect.SetActive(false);
        //SceneManager.LoadScene("Merge");
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
        HangingManager.isCompulsoryEnd = true;

        attackerDialogEvent.SetSituationDialogEvent(UnityEngine.Random.Range(1, 11), 3f);

        hangingTimer.SetTimer(true);
        cameraMoveScript.SetPossibleMove(true);
        attackerMouseMove.SetPossibleTodesstrafe(true);
        dialogWindowController.SetEnabled(true);
        //EventManager.instance.postNotification("amnesty", this, null);
        //EventManager.instance.postNotification("moveCameraToDesk", this, null);

    }

    void InitialAttacker()
    {
        if (attackerMouseMove != null)
        {
            Destroy(attackerMouseMove.gameObject);
        }
        GameObject attacker = Instantiate(attackerPrefab);
        attackerMouseMove = attacker.GetComponent<AttackerMouseMove>();
        attackerInfo = attacker.GetComponent<AttackerInfo>();
        attackerDialogEvent = attacker.GetComponent<AttackerDialogEvent>();

        isTodesstrafe = false;
        isAmnesty = false;
        hangingTimer.Initial();
    }
}