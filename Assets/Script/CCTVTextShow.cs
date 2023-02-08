using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Globalization;

public class CCTVTextShow : MonoBehaviour
{
    private TextMeshPro textMeshProUGUI;

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        DateTime date = System.Convert.ToDateTime("2132/2/3");
        date.AddDays(HangingManager.day);

        string text = "CAMERA" + 1 + "\n" // 1 대신 사형수 idx
            + "PLAY ▶" + "\n"
            + date.ToString("yy/MM/dd ") + " " + date.ToString("ddd", new CultureInfo("en-US"));

        textMeshProUGUI.text = text.Replace("\\n", "\n");
    }
}
