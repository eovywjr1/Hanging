using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowButtonAddListner : MonoBehaviour
{
    Button button;
    LineManager lineManager;

    private void Awake()
    {
        button = GetComponent<Button>();
        lineManager = FindObjectOfType<LineManager>();
    }

    private void Start()
    {
        button.onClick.AddListener(lineManager.CreateLine);
    }

    public void Deactivate()
    {
        Destroy(gameObject);
    }
}