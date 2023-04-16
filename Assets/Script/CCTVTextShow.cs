using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Globalization;
using Unity.VisualScripting;

public class CCTVTextShow : MonoBehaviour, IListener
{
    private TextMeshPro textMeshProUGUI;

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        EventManager.instance.addListener("updateAttackerCountCCTV", this);
        UpdateText(1);
    }

    public void UpdateText(int attackerCount)
    {
        DateTime date = System.Convert.ToDateTime("2132/2/1");
        date = date.AddDays(HangingManager.day - 1);

        string text = "CAMERA" + attackerCount.ToString() + "\n"
            + "PLAY ��" + "\n"
            + date.ToString("yy/MM/dd ") + " " + date.ToString("ddd", new CultureInfo("en-US"));

        textMeshProUGUI.text = text.Replace("\\n", "\n");
    }

    public void OnEvent(string eventType, Component sender, object parameter = null)
    {
        switch (eventType)
        {
            case "updateAttackerCountCCTV":
                UpdateText((int)parameter);
                break;
        }
    }
}
