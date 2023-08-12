using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.IO;

public class DialogUpdateAndEvent : MonoBehaviour, IListener
{
    DialogCSVReader dialogReader = new DialogCSVReader();
    DialogBubbleController dialogBubbleController;
    DialogWindowController dialogWindowController;
    HangingManager hangingManager;
    private BossHand _bossHand = null;

    private Dictionary<string, List<List<string>>> compulsoryT, situationD;

    bool timeover;
    public bool clickAttacker, todesstrafe, amnesty, moveCameraToDesk, activeGuide, deactiveGuide, clickBasicJudgementGuide; // waituntil 위해서 public

    string conditionName;

    private void Awake()
    {
        dialogBubbleController = GetComponent<DialogBubbleController>();
        dialogWindowController = FindObjectOfType<DialogWindowController>();
        hangingManager = FindObjectOfType<HangingManager>();
        { 
            _bossHand = FindObjectOfType<BossHand>();
            Debug.Assert(_bossHand, "조민수 : bossHand 스크립트가 없어서 확인부탁드립니다.");
        }

        string fileName = HangingManager.day + "DayCompulsoryDialog";
        string filePath = "Assets/Resources/" + fileName + ".csv";
        FileInfo fileInfo = new FileInfo(filePath);
        if (fileInfo.Exists == false)
        {
            Debug.Assert(false, "조민수 comment : " + filePath + "이 없습니다 파일을 추가해주세요.");
            return;
        }

        compulsoryT = dialogReader.Read(fileName);
        situationD = dialogReader.Read("SituationDialog");
    }

    void Start()
    {
        EventManager.instance.addListener("dialogEvent", this);

        string id = HangingManager.day + "000";
        StartCoroutine(UpdateDialogCompulsory(id));
    }

    IEnumerator UpdateDialogCompulsory(string id)
    {
        if (compulsoryT == null)
            yield break;

        while (compulsoryT.ContainsKey(id))
        {
            yield return StartCoroutine(UpdateDialog(compulsoryT[id]));

            id = (StringToInt(id) + 1).ToString();
        }

        hangingManager.EndCompulsory();

        yield return null;
    }

    IEnumerator UpdateDialogSituation(string id)
    {
        yield return StartCoroutine(UpdateDialog(situationD[id]));

        yield return null;
    }

    IEnumerator UpdateDialog(List<List<string>> list)
    {
        IEnumerator dialogTimerCoroutine;

        foreach (var i in list)
        {
            Initialize();

            dialogBubbleController.CreateDialogBubble(StringToInt(i[1]), i[2]);

            dialogTimerCoroutine = NextDialogTimer();
            StartCoroutine(dialogTimerCoroutine);
            yield return new WaitUntil(NextDialog);
            StopCoroutine(dialogTimerCoroutine);
            yield return StartCoroutine(Timer(0.2f)); // 검토 //

            if ((i.Count > 3) && (i[3].Equals("") == false))
            {
                conditionName = i[3];
                EventManager.instance.postNotification("possible" + conditionName, this, null);
                yield return new WaitUntil(() =>
                {
                    if (this.GetType().GetField(conditionName).GetValue(this).ConvertTo<bool>())
                    {
                        this.GetType().GetField(conditionName).SetValue(this, false);
                        return true;
                    }

                    return false;
                });
            }

            if ((i.Count > 4) && (i[4].Equals("") == false))
                Invoke(i[4], 0f);
        }
    }

    bool NextDialog()
    {
        if ((timeover) || (Input.GetMouseButtonDown(0) && (Input.mousePosition.x >= 840) && dialogWindowController.dialogCanvas.enabled))
        {
            return true;
        }

        return false;
    }

    IEnumerator NextDialogTimer()
    {
        yield return StartCoroutine(Timer(1f));

        timeover = true;
    }

    public IEnumerator SetSituationDialog(int id, float time)
    {
        yield return StartCoroutine(Timer(time));

        StartCoroutine(UpdateDialogSituation(id.ToString()));
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
    }

    public void SetValueByName(string name)
    {
        this.GetType().GetField(name).SetValue(this, true);
    }

    public IEnumerator dialogUnsubmitBadge(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        bool isBadgeSumit = _bossHand.isSubmit;

        if (isBadgeSumit == false)
        {
            if (time == 10f)
                StartCoroutine(SetSituationDialog(UnityEngine.Random.Range(58, 60), 0));
        }
    }

    void Initialize()
    {
        timeover = false;
    }

    int StringToInt(string str)
    {
        return int.Parse(str);
    }

    private void badgeCountDownDialog()
    {
        StartCoroutine(SetSituationDialog(60, 0));
    }

    private void badgeSecondCountDownDialog()
    {
        StartCoroutine(_bossHand.checkSubmit(2f, (isSubmit) =>
        {
            if (isSubmit == false)
                return;
        }));

        StartCoroutine(SetSituationDialog(60, 0));
    }

    private void badgeLastSecondCountDownDialog()
    {
        StartCoroutine(_bossHand.checkSubmit(2f, (isSubmit) =>
        {
            if (isSubmit)
                StartCoroutine(SetSituationDialog(61, 0));
            else
                StartCoroutine(SetSituationDialog(62, 0));
        }));
    }

    private void gameOverForUnSubmitBage()
    {
        StartCoroutine(FindObjectOfType<GameManager>().endGame());
    }

    private void showIllegalMoveGuide()
    {
        FindObjectOfType<GuideButton>().showIllegalMoveGuide();
    }

    public void OnEvent(string eventType, Component sender, object parameter = null)
    {
        if (sender == this)
            return;

        if (parameter.GetType() == typeof(int))
            StartCoroutine(SetSituationDialog(Convert.ToInt32(parameter), 0));

        switch (parameter)
        {
            case "clickAttacker":
                clickAttacker = true;
                break;
            case "moveCameraToDesk":
                moveCameraToDesk = true;
                break;
            case "createAttacker":
                StartCoroutine(SetSituationDialog(UnityEngine.Random.Range(1, 11), 3f));
                break;
            case "todesstrafe":
                todesstrafe = true;
                break;
            case "amnesty":
                amnesty = true;
                break;
            case "activeGuide":
                activeGuide = true;
                break;
            case "deactiveGuide":
                deactiveGuide = true;
                break;
            case "clickBasicJudgementGuide":
                clickBasicJudgementGuide = true;
                break;
        }
    }
}
