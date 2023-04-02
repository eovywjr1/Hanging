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
    DialogWindowController dialogWindowController;
    ContentSizeFitter csf;

    private void Awake()
    {
        dialogWindowController = FindObjectOfType<DialogWindowController>();
        csf = GetComponent<ContentSizeFitter>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.V)) CreateDialogBubble( 1 , "qweqweeqwweqeqwqweqwe");
    }

    GameObject GetDialogBubble( int id ) // 플레이어(0), 사형수(1), 보스(2)
    {
        return Instantiate( dialogBubblePrefab[id], dialogBubblePrefab[id].transform.position, Quaternion.identity, transform);
    }

    public void CreateDialogBubble( int id , string str )
    {
        dialogWindowController.VisibleDialogWindow();
        dialogBubbleList.Add( GetDialogBubble( id ) );
        //길면 줄바꿈 함수 삽입 자리//
        DialogTextShow( str );
        BubbleSetSizeSet(str);
        SetPositionDialogBubble(260, id);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)csf.transform);
    }

    void SetPositionDialogBubble ( float startY, int id)
    {
        RectTransform dialogRectTransform = dialogBubbleList[dialogBubbleList.Count - 1].GetComponent<RectTransform>();
        float y = startY - (dialogBubbleList.Count - 1) * 40;
        float x;

        if (id == 0) x = 160 - dialogRectTransform.sizeDelta.x / 2;
        else x = -160 + dialogRectTransform.sizeDelta.x / 2;

        dialogRectTransform.localPosition = new Vector2(x, y);
    }

    void DialogTextShow(string str)
    {
        dialogBubbleList[dialogBubbleList.Count - 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = str;
    }

    void BubbleSetSizeSet(string str)
    {
        RectTransform dialogRectTransform = dialogBubbleList[dialogBubbleList.Count - 1].GetComponent<RectTransform>();
        //float x = str.Length * 14;
        //Vector2 vector2 = new Vector2(x, dialogRectTransform.sizeDelta.y);
        //dialogRectTransform.sizeDelta = vector2;
        //dialogRectTransform.sizeDelta = dialogBubbleList[dialogBubbleList.Count - 1].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
        StartCoroutine(BubbleSetSizeSet1());
        //dialogBubbleList[dialogBubbleList.Count - 1].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = vector2;
    }

    IEnumerator BubbleSetSizeSet1()
    {
        yield return new WaitForSecondsRealtime(0.1f * Time.deltaTime);
        RectTransform dialogRectTransform = dialogBubbleList[dialogBubbleList.Count - 1].GetComponent<RectTransform>();
        dialogRectTransform.sizeDelta = dialogBubbleList[dialogBubbleList.Count - 1].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
    }
}
