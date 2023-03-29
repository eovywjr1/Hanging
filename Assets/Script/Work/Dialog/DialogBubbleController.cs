using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogBubbleController : MonoBehaviour
{
    [SerializeField] GameObject []dialogBubblePrefab;
    List<GameObject> dialogBubbleList = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.V)) CreateDialogBubble(1);
    }

    GameObject GetDialogBubble( int id ) // 플레이어(0), 사형수(1), 보스(2)
    {
        return Instantiate( dialogBubblePrefab[id], dialogBubblePrefab[id].transform.position, Quaternion.identity, transform );
    }

    void CreateDialogBubble( int id )
    {
        dialogBubbleList.Add( GetDialogBubble( id ) );
        //이 자리에 버블 크기 설정 함수 넣어야 함//
        SetPositionDialogBubble( 260, dialogBubbleList.Count - 1, id );
    }

    void SetPositionDialogBubble ( float startY, int index , int id)
    {
        RectTransform dialogRectTransform = dialogBubbleList[index].gameObject.GetComponent<RectTransform>();
        float y = startY - index * 40;
        float x;

        if (id == 0) x = 160 - dialogRectTransform.sizeDelta.x / 2;
        else x = -160 + dialogRectTransform.sizeDelta.x / 2;

        dialogRectTransform.localPosition = new Vector2(x, y);
    }
}
