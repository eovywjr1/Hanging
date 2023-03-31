using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeTextTexture : MonoBehaviour
{
    public bool mentTureORFalse;
    public bool afterClick;
    public int lieORinfoErrorValue;
    public string lastMent;//위증또는 정보오류일때 마지막 문자열 비교
    CursorScript cursorScript;

    private void Start()
    {
        cursorScript= FindObjectOfType<CursorScript>();
        afterClick = false;

    }
    private void OnMouseEnter()
    {
        if (cursorScript.penCursor && afterClick==false)
        {
            Debug.Log("마우스 인");
            gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color32(239,239,239,255);
            //gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().color = Color.white;

        }
    }

    private void OnMouseExit()
    {
        if(afterClick == false)
        {
            gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            //gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().color = Color.black;
        }

    }
    
}
