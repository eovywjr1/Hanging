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

        string text = "CAMERA" + HangingManager.attackerCount.ToString() + "\n"
            + "PLAY ¢º" + "\n"
            + date.ToString("yy/MM/dd ") + " " + date.ToString("ddd", new CultureInfo("en-US"));

        textMeshProUGUI.text = text.Replace("\\n", "\n");
    }
}
