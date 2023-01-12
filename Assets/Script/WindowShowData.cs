using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WindowShowData : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;
    private string text;

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        OffenderData offenderdata = Offender.offenderData;

        text = "      가해자: " + offenderdata.fname + " " + offenderdata.name + "\n"
            + "      죄목: " + offenderdata.crime + "\n"
            + "      발생 장소: " + offenderdata.grade + " 등급" + "\n"
            + "      피해자: " + offenderdata.vfname + " " + offenderdata.vname + "\n"
            + "      경위: " + offenderdata.detail;

        textMeshProUGUI.text = text.Replace("\\n", "\n");
    }
}
