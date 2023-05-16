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
    private UiManager _uiManager;

    [SerializeField] GameObject convertEffect, attackerPrefab;
    public ScrollViewController scrollViewController;

    public bool isTodesstrafe, isAmnesty, isActiveAsk;
    public static bool isCompulsoryEnd;
    public bool isStatementWrongProcess;
    int _attackerCount = 1;
    public int attackerCount;


    private int _judgeCount = 0;
    public int judgeCount
    {
        get { return _judgeCount; }
        set { 
            _judgeCount = value;
            EventManager.instance.postNotification("updateAttackerCountCCTV", this, _judgeCount + 1);
        }
    }
    private int _correctJudgeCount = 0;
    private int _discorrectJudgeCount = 0;
    private int _discorrectAndTodesstrafedPersonCount = 0;

    public static int day = 3;
    static bool isCorrect = true;

    private void Awake()
    {
        dialogWindowController = FindObjectOfType<DialogWindowController>();
        hangingTimer = FindObjectOfType<HangingTimer>();
        analogGlitch = FindObjectOfType<AnalogGlitch>();
        _uiManager = FindObjectOfType<UiManager>();

        createAttacker();
        scrollViewController=FindObjectOfType<ScrollViewController>();
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

        if (isActiveAsk && attackerInfo.recordData.attackerData["ask"].Equals("1") == false)
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

        Debug.Log("현재범죄는 뭘까용"+attackerInfo.recordData.currentState[2]);
    }

    public void Todesstrafe()
    {
        isTodesstrafe = true;

        //���� �Ǻ�//
        if (checkTodesstrafe(0)) Debug.Log("True");
        else Debug.Log("False");
        Debug.Log("����");

        EndTodesstrafe();
    }

    public void Amnesty()
    {
        isAmnesty = true;

        //���� �Ǻ�//
        if (checkTodesstrafe(1)) Debug.Log("True");
        else Debug.Log("False");
        Debug.Log("����");

        EndTodesstrafe();
    }

    private bool checkTodesstrafe(int mode)
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
        judgeCount++;

        StartCoroutine(ConvertAttackerEffect());

        createAttacker();
    }

    IEnumerator ConvertAttackerEffect()
    {
        convertEffect.SetActive(true);

        yield return new WaitForSecondsRealtime(1.5f);

        convertEffect.SetActive(false);
    }

    IEnumerator StartHoldOutHands()
    {
        yield return new WaitForSecondsRealtime(1.25f);
        EventManager.instance.postNotification("badge", this, null);
        EventManager.instance.postNotification("dialogEvent", this, UnityEngine.Random.Range(53, 58));
        yield return new WaitForSecondsRealtime(3.0f);
        isStatementWrongProcess = false;
    }

    public IEnumerator StartStateWrong() //진술서 오답일 경우 노이즈 출력 & 뱃지 회수 
    {
        isStatementWrongProcess = true;

        analogGlitch._isGlitch = true;
        yield return new WaitForSecondsRealtime(0.75f);
        analogGlitch._isGlitch = false;

        StartCoroutine(StartHoldOutHands());
    }

    public void EndCompulsory()
    {
        isCompulsoryEnd = true;

        EventManager.instance.postNotification("dialogEvent", this, "createAttacker");
        EventManager.instance.postPossibleEvent();

        hangingTimer.SetTimer(true);
        dialogWindowController.SetEnabled(true);
    }

    void createAttacker()
    {
        if (attackerMouseMove != null)
            Destroy(attackerMouseMove.gameObject);

        GameObject attacker = Instantiate(attackerPrefab);
        attackerMouseMove = attacker.GetComponent<AttackerMouseMove>();
        attackerInfo = attacker.GetComponent<AttackerInfo>();

        if (isCompulsoryEnd)
        {
            Debug.Log("캐릭터 생성중?");
            EventManager.instance.postNotification("todesstrafe", this, null);
            EventManager.instance.postNotification("amnesty", this, null);
        }

        isTodesstrafe = false;
        isAmnesty = false;
        isActiveAsk = false;


    }

    public HangingInfoWrapper getHangingInfo()
    {
        return new HangingInfoWrapper(day, _judgeCount, _correctJudgeCount, _discorrectJudgeCount, _discorrectAndTodesstrafedPersonCount);
    }

    public IEnumerator endDay()
    {
        _uiManager.hideScreenCanvas();

        yield return StartCoroutine(ConvertAttackerEffect());

        _uiManager.showScreenCanvas();
        _uiManager.showDominantImage();
    }

    public void convertSceneNextDay()
    {
        //퇴근길 로드로 변경해야 함
        //퇴근길이 끝난 후에 day 증가하는 것으로 변경해야 함
        day++;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnEvent(string eventType, Component sender, object parameter = null)
    {
        switch (eventType)
        {
            case "amnesty":
                Amnesty();
                break;
            case "todesstrafe":
                Todesstrafe();
                break;
            case "activeAsk":
                isActiveAsk = true;
                break;
        }
    }
}