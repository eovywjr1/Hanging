using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offender : MonoBehaviour
{
    private LineManager lineManager;
    private bool isCreateLine;

    private void Start()
    {
        lineManager = FindObjectOfType<LineManager>();
    }

    private void OnMouseDown()
    {
        if (!isCreateLine)
        {
            lineManager.CreateLine(transform);
            isCreateLine = true;
        }
    }
}
