using Kino;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using hanging;

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

    [SerializeField] private GameObject badgePrefab;
    [SerializeField] private GameObject badgeWrap;
    private bool isEndCompulsoryDialog;
    private int _correctJudgeCount = 0;
    private int _discorrectJudgeCount = 0;
    private int _discorrectAndTodesstrafedPersonCount = 0;

    public static int day = 1;
#if UNITY_EDITOR
    public int debug_day = 1;
#endif

    private static int badgeCount = 3;

    public enum EnableGimmick
    {
        StaButton
    }


    private void Awake()
    {
#if UNITY_EDITOR
        day = debug_day;
#endif

        dialogWindowController = FindObjectOfType<DialogWindowController>();
        hangingTimer = FindObjectOfType<HangingTimer>();
        analogGlitch = FindObjectOfType<AnalogGlitch>();
        _uiManager = FindObjectOfType<UiManager>();
        scrollViewController=FindObjectOfType<ScrollViewController>();
    }

    void Start()
    {
        EventManager.instance.addListener("amnesty", this);
        EventManager.instance.addListener("todesstrafe", this);
        EventManager.instance.addListener("executeAsk", this);

        createAttacker();

        if (CheckEnableGimmick(EnableGimmick.StaButton))
        { 
            OnStaButton();
            
        }

        spawnBadge(badgeCount);
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

        //ÔøΩÔøΩÔøΩÔøΩ ÔøΩ«∫ÔøΩ//
        if (checkCorrectTodesstrafe(0)) 
            Debug.Log("True");

        Debug.Log("ÔøΩÔøΩÔøΩÔøΩ");

        EndTodesstrafe();
    }

    public void Amnesty()
    {
        //ÔøΩÔøΩÔøΩÔøΩ ÔøΩ«∫ÔøΩ//
        if (checkCorrectTodesstrafe(1)) 
            Debug.Log("True");

        Debug.Log("ÔøΩÔøΩÔøΩÔøΩ");

        EndTodesstrafe();
    }

    private bool checkCorrectTodesstrafe(int mode)
    {
        if (isEndCompulsoryDialog == false || mode == attackerInfo.recordData.isHanging)
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

    public IEnumerator StartStateWrong() //ÏßÑÏà†?Ñú ?ò§?ãµ?ùº Í≤ΩÏö∞ ?Ö∏?ù¥Ï¶? Ï∂úÎ†• & Î±ÉÏ?? ?öå?àò 
    {
        isStatementWrongProcess = true;

        analogGlitch._isGlitch = true;
        yield return new WaitForSecondsRealtime(0.75f);
        analogGlitch._isGlitch = false;

        StartCoroutine(StartHoldOutHands());
    }

    public void EndCompulsory()
    {
        isEndCompulsoryDialog = true;

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

        if (isEndCompulsoryDialog)
        {
            EventManager.instance.postPossibleEvent();

            Rope rope = attacker.transform.Find("rope").GetComponent<Rope>();
            Debug.Assert(rope, "not find rope");
            if (rope)
                rope.isPossibleCut = true;
        }

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
        
      
        //Ï°∞Î?ºÏàò comment : 7?ùºÏ∞®Ïóê ?ôòÍ∞? ?Çò?ò§Í≥? Í≤åÏûÑ Ï¢ÖÎ£å?äî ?ùº?ã® ?ûÑ?ãú, Ï∂îÌõÑ?óê Î≥?Í≤ΩÍ???ä•?Ñ± ?ûà?ùå
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
        //?á¥Í∑ºÍ∏∏ Î°úÎìúÎ°? Î≥?Í≤ΩÌï¥?ïº ?ï®
        //?á¥Í∑ºÍ∏∏?ù¥ ?Åù?Çú ?õÑ?óê day Ï¶ùÍ???ïò?äî Í≤ÉÏúºÎ°? Î≥?Í≤ΩÌï¥?ïº ?ï®
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
        
        if (staButton == null)
            return;
        staButton.SetActive(true);
      
    }

    public void searchReport()
    {

    }

    public bool checkEndCompulsoryDialog()
    {
        return isEndCompulsoryDialog;
    }

    private void spawnBadge(int size)
    {
        if (badgePrefab == null)
        {
            Debug.Assert(false, "not insert BadgePrefab");
            return;
        }

        if (badgeWrap == null)
        {
            Debug.Assert(false, "not insert BadgeWrap");
            return;
        }

        Transform badgeTransform = badgeWrap.transform;
        for (int index = 0; index < size; ++index)
        {
            Vector3 vector3 = badgeTransform.position;
            vector3.x = index - 1;
            Instantiate(badgePrefab, vector3, Quaternion.identity, badgeTransform);
        }
    }

    public bool CheckEnableGimmick(EnableGimmick enableGimmickType)
    {
        switch (enableGimmickType)
        {
            case EnableGimmick.StaButton:
                {
                    if (day >= 6)
                        return true;

                    break;
                }
        }

        return false;
    }
}