using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogBubbleController : MonoBehaviour
{
    [SerializeField] GameObject []dialogBubblePrefab;
    List<GameObject> dialogBubbleList = new List<GameObject>();
    List<RectTransform> dialogBubbleRecttransformList = new List<RectTransform>();
    DialogWindowController dialogWindowController;
    const int perBubbleHeight = 50;
    [SerializeField] Scrollbar scrollbar;

    private void Awake()
    {
        dialogWindowController = FindObjectOfType<DialogWindowController>();
    }

    GameObject GetDialogBubble( int id ) // 플레이어(0), 사형수(1), 보스(2)
    {
        return Instantiate( dialogBubblePrefab[id], dialogBubblePrefab[id].transform.position, Quaternion.identity, transform);
    }

    public void CreateDialogBubble( int id , string str )
    {
        if (dialogBubbleList == null)
            return;

        if (dialogWindowController == null)
            dialogWindowController.VisibleDialogWindow();

        if (id >= 0 && id <= 2)
            dialogBubbleList.Add(GetDialogBubble(id).transform.GetChild(0).gameObject);

        int bubbleIndex = dialogBubbleList.Count - 1;
        if (bubbleIndex >= 0)
            dialogBubbleRecttransformList.Add(dialogBubbleList[bubbleIndex].GetComponent<RectTransform>());

        string modifiedString = str.Replace("\"", "");
        DialogTextShow(modifiedString, bubbleIndex);
        SetBubbleSetSize(id, bubbleIndex);
    }

    void SetPositionDialogBubble (int id, int index)
    {
        RectTransform dialogRectTransform = dialogBubbleRecttransformList[index];
        float x;

        if (id == 0)
            x = 155 - dialogRectTransform.sizeDelta.x / 2;
        else
            x = -155 + dialogRectTransform.sizeDelta.x / 2;

        dialogRectTransform.localPosition = new Vector2(x, 0);
    }

    void DialogTextShow(string str, int index)
    {
        dialogBubbleList[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = str;
    }

    void SetBubbleSetSize(int id, int index)
    {
        StartCoroutine(SetBubble(id, index));
    }

    void SetScrollValue()
    {
        scrollbar.value = 0;
    }

    IEnumerator SetBubble(int id, int index)
    {
        yield return new WaitForSecondsRealtime(Time.deltaTime);
        dialogBubbleRecttransformList[index].sizeDelta = dialogBubbleList[index].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;

        yield return new WaitForSecondsRealtime(Time.deltaTime); //생성 딜레이 후 set 적용
        SetPositionDialogBubble(id, index);
        SetScrollValue();
    }
}
