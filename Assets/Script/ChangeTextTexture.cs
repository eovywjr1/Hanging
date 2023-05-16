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
    public string lastMent;//�����Ǵ� ���������϶� ������ ���ڿ� ��
    CursorScript cursorScript;

    private void Start()
    {
        cursorScript= FindObjectOfType<CursorScript>();


    }
    private void OnMouseEnter()
    {
        if (cursorScript.penCursor && afterClick==false)
        {
            Debug.Log("���콺 ��");
            gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color32(239, 239, 239, 255);
        }
    }

    private void OnMouseExit()
    {
        if(afterClick == false)
        {
            gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        }

    }
    
}
