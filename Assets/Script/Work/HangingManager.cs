using Kino;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    [SerializeField] GameObject convertEffect, nextDayEffect,attackerPrefab;
    public ScrollViewController scrollViewController;

    public bool isTodesstrafe, isExecuteAsk;
    public bool isStatementWrongProcess;

    public GameObject staButton;

    private int _judgeCount = 0;
    public int judgeCount
    {
        get { return _judgeCount; }
        set { 
            _judgeCount = value;
            EventManager.instance.postNotification("updateAttackerCountCCTV", this, _judgeCount + 1);
        }
    }
    private bool isCompulsoryEnd;
    private int _correctJudgeCount = 0;
    private int _discorrectJudgeCount = 0;
    private int _discorrectAndTodesstrafedPersonCount = 0;

    public static int day = 8;
    private static int badgeCount = 3;

    private void Awake()
    {
        dialogWindowController = FindObjectOfType<DialogWindowController>();
        hangingTimer = FindObjectOfType<HangingTimer>();
        analogGlitch = FindObjectOfType<AnalogGlitch>();
        _uiManager = FindObjectOfType<UiManager>();

        scrollViewController=FindObjectOfType<ScrollViewController>();
    }

    private void Start()
    {
        createAttacker();

        EventManager.instance.addListener("amnesty", this);
        EventManager.instance.addListener("todesstrafe", this);
        EventManager.instance.addListener("executeAsk", this);

        if (day >= 6)
            OnStaButton();

        FindObjectOfType<BadgeManager>().spawnBadge(badgeCount);
    }

    public void EndTodesstrafe()
    {
        DestroyAllLineAndWindow();
        NextAttacker();

        if ((isExecuteAsk) && (attackerInfo.checkAttackerReplyAsk() == false))
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
        if (checkCorrectTodesstrafe(0)) 
            Debug.Log("True");

        Debug.Log("����");

        EndTodesstrafe();
    }

    public void Amnesty()
    {
        //���� �Ǻ�//
        if (checkCorrectTodesstrafe(1)) 
            Debug.Log("True");

        Debug.Log("����");

        EndTodesstrafe();
    }

    private bool checkCorrectTodesstrafe(int mode)
    {
        if (isCompulsoryEnd == false || mode == attackerInfo.recordData.isHanging)
        {
            Debug.Log(attackerInfo.recordData.isHanging);

            ++_correctJudgeCount;

            return true;
        }
        else
        {
            Debug.Log(attackerInfo.recordData.isHanging);

            StartCoroutine(StartHoldOutHands());

            ++_discorrectJudgeCount;
            if (mode == 0)
                ++_discorrectAndTodesstrafedPersonCount;

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

    IEnumerator EndDayEffect()
    {
        nextDayEffect.SetActive(true);

        yield return new WaitForSecondsRealtime(2.2f);

        nextDayEffect.SetActive(false);
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
            EventManager.instance.postPossibleEvent();

        isTodesstrafe = false;
        isExecuteAsk = false;
    }

    public HangingInfoWrapper getHangingInfo()
    {
        return new HangingInfoWrapper(day, _judgeCount, _correctJudgeCount, _discorrectJudgeCount, _discorrectAndTodesstrafedPersonCount);
    }

    public IEnumerator endDay()
    {
        _uiManager.hideScreenCanvas();
        
      
        //조민수 comment : 7일차에 환각 나오고 게임 종료는 일단 임시, 추후에 변경가능성 있음
        if (day == 7)
        {
            StartCoroutine(FindObjectOfType<GameManager>().endGame());
        }
        else
        {
            yield return StartCoroutine(EndDayEffect());

            _uiManager.showScreenCanvas();
            _uiManager.showDominantImage();
        }
    }

    public void convertSceneNextDay()
    {
        //퇴근길 로드로 변경해야 함
        //퇴근길이 끝난 후에 day 증가하는 것으로 변경해야 함
        day++;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void subtractBadgeCount()
    {
        --badgeCount;
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
            case "executeAsk":
                isExecuteAsk = true;
                break;
        }
    }

    private void OnStaButton()
    {
        staButton.SetActive(true);
    }

    public void searchReport()
    {

    }
}