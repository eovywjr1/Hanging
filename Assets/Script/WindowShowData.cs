using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WindowShowData : MonoBehaviour
{
    protected WindowSetSize windowSetSize;
    protected RecordData recordData;

    protected void Awake()
    {
        windowSetSize = transform.parent.GetComponent<WindowSetSize>();
    }

    //텍스트 설정//
    public void SetText(string str)
    {
        GetComponent<TextMeshProUGUI>().text = str.Replace("\\n", "\n");
    }

    ////창 크기 조절//
    protected void SetTextSize(float width)
    {
        windowSetSize.SetSize(width);
    }
}
