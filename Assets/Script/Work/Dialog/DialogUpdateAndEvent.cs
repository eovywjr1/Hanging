using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class DialogUpdateAndEvent : MonoBehaviour, IListener
{
    DialogCSVReader dialogReader = new DialogCSVReader();
    DialogBubbleController dialogBubbleController;
    DialogWindowController dialogWindowController;
    HangingManager hangingManager;

    private Dictionary<string, List<List<string>>> compulsoryT, situationD;

    bool timeover;
    public bool clickAttacker, todesstrafe, amnesty, moveCameraToDesk, submitBadge; // waituntil 위해서 public

    string conditionName;

    private void Awake()
    {
        dialogBubbleController = GetComponent<DialogBubbleController>();
        dialogWindowController = FindObjectOfType<DialogWindowController>();
        hangingManager = FindObjectOfType<HangingManager>();

        string fileName = HangingManager.day + "DayCompulsoryDialog";
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
                if (i[3] != "InputKeyG" && i[3] != "ClickBasicJudgementGuide")  // 임시 > 이벤트 연결 후 삭제
                {
                    conditionName = i[3];
                    yield return new WaitUntil(() =>
                    {
                        EventManager.instance.postNotification("possible" + conditionName, this, null); //csv에서 possible 뒤 첫 글자는 무조건 lower

                        if (this.GetType().GetField(conditionName).GetValue(this).ConvertTo<bool>())
                            return true;

                        return false;
                    });
                }
            }

            if ((i.Count > 4) && (i[4].Equals("") == false))
                EventManager.instance.postNotification(i[4], this, null);
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

    void Initialize()
    {
        timeover = false;
    }

    int StringToInt(string str)
    {
        return int.Parse(str);
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
        }
    }
}
