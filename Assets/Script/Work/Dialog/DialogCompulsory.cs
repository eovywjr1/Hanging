using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogCompulsory : MonoBehaviour
{
    DialogCSVReader dialogReader = new DialogCSVReader();
    DialogBubbleController dialogBubbleController;
    private Dictionary<string, List<List<string>>> compulsoryT;
    bool timeover = false;
   
    private void Awake()
    {
        dialogBubbleController = GetComponent<DialogBubbleController>();

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

        while (compulsoryT.ContainsKey(id))
        {
            foreach (var i in compulsoryT[id])
            {
                dialogBubbleController.CreateDialogBubble(StringToInt(i[1]), i[2]);

                StartCoroutine(NextDialogTimer());
                yield return new WaitUntil(NextDialog);
                yield return new WaitForSecondsRealtime(0.2f); // 검토 //

                if (i[3].Equals("") == false)
                    yield return new WaitUntil(() => (bool)this.GetType().GetMethod(i[3]).Invoke(this, null));
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
        if (timeover || Input.GetMouseButtonDown(0))
        {
            timeover = false;
            StopCoroutine(NextDialogTimer());

            return true;
        }

        return false;
    }

    IEnumerator NextDialogTimer()
    {
        timeover = false;

        yield return new WaitForSecondsRealtime(1f);

        timeover = true;
    }

    public bool ActiveGuide()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("true");
            return true;
        }

        return false;
    }
}
