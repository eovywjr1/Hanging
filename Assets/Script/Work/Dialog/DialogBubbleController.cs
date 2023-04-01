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

    private void Awake()
    {
        dialogWindowController = FindObjectOfType<DialogWindowController>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.V)) CreateDialogBubble( 1 , "qweqweeqwweqeqwqweqwe");
    }

    GameObject GetDialogBubble( int id ) // 플레이어(0), 사형수(1), 보스(2)
    {
        return Instantiate( dialogBubblePrefab[id], dialogBubblePrefab[id].transform.position, Quaternion.identity, transform );
    }

    void CreateDialogBubble( int id , string str )
    {
        dialogWindowController.VisibleDialogWindow();
        dialogBubbleList.Add( GetDialogBubble( id ) );
        DialogTextShow( str );
        BubbleSetSizeSet(str);
        SetPositionDialogBubble(260, id);
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
        float x = str.Length * 11;
        Vector2 vector2 = new Vector2(x, dialogRectTransform.sizeDelta.y);
        dialogRectTransform.sizeDelta = vector2;
        dialogBubbleList[dialogBubbleList.Count - 1].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = vector2;
    }
}
