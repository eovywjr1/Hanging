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
    protected void SetTextSize(int maxLength)
    {
        float width = 2.3f + 0.15f * (maxLength - 3);
        windowSetSize.SetSize(width);
    }

    virtual protected void Showdata() { }
}
