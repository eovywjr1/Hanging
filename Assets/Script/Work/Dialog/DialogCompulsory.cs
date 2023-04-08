using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class DialogCompulsory : MonoBehaviour
{
    DialogCSVReader dialogReader = new DialogCSVReader();
    DialogBubbleController dialogBubbleController;
    HangingManager hangingManager;

    private Dictionary<string, List<List<string>>> compulsoryT;

    bool timeover = false;
    public bool isCickAttacker = false;
   
    private void Awake()
    {
        dialogBubbleController = GetComponent<DialogBubbleController>();
        hangingManager = FindObjectOfType<HangingManager>();

        string fileName = HangingManager.day + "일차 대본";
        compulsoryT = dialogReader.Read(fileName);
    }

    void Start()
    {
        StartCoroutine(Dialog());
    }

    IEnumerator Dialog()
    {
        string id = HangingManager.day + "000";
        IEnumerator dialogTimerCoroutine;

        while (compulsoryT.ContainsKey(id))
        {
            foreach (var i in compulsoryT[id])
            {
                Initialize();

                dialogBubbleController.CreateDialogBubble(StringToInt(i[1]), i[2]);

                dialogTimerCoroutine = NextDialogTimer();
                StartCoroutine(dialogTimerCoroutine);
                yield return new WaitUntil(NextDialog);
                StopCoroutine(dialogTimerCoroutine);
                yield return new WaitForSecondsRealtime(0.2f); // 검토 //

                if ((i.Count > 3) && (i[3].Equals("") == false))
                    yield return new WaitUntil(() => (bool)this.GetType().GetMethod(i[3]).Invoke(this, null));

                if ((i.Count > 4) && (i[4].Equals("") == false))
                    this.GetType().GetMethod(i[4]).Invoke(this, null);
            }

            id = IntToString(StringToInt(id) + 1);
        }

        yield return null;
    }

    int StringToInt(string str)
    {
        return int.Parse(str);
    }
    
    string IntToString(int i)
    {
        return i.ToString();
    }

    bool DialogCondition(string str)
    {
        return true;
    }

    bool NextDialog()
    {
        if ((timeover) || (Input.GetMouseButtonDown(0) && (Input.mousePosition.x >= 650)))
        {
            return true;
        }

        return false;
    }

    IEnumerator NextDialogTimer()
    {
        yield return new WaitForSecondsRealtime(1f);

        timeover = true;
    }

    void Initialize()
    {
        timeover = false;
    }

    public bool InputKeyG()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            return true;
        }

        return false;
    }

    public bool ClickAttacker()
    {
        if (isCickAttacker)
        {
            return true;
        }

        return false;
    }

    public bool ClickBasicJudgementGuide()
    {
        if (true)
            return true;

        //return false;
    }

    public bool Todesstrafe()
    {
        if (hangingManager.isTodesstrafe)
        {
            return true;
        }

        return false;
    }

    public bool Amnesty()
    {
        if(hangingManager.isAmnesty)
        {
            return true;
        }

        return false;
    }

    public bool InputKeySpace()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }

        return false;
    }
}
