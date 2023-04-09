using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogWindowController : MonoBehaviour
{
    [SerializeField] GameObject dialogWindow;
    public Canvas dialogCanvas;

    bool isEnabled;

    private void Awake()
    {
        dialogCanvas = dialogWindow.transform.parent.GetComponent<Canvas>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && isEnabled)
        {
            dialogCanvas.enabled = !dialogCanvas.enabled;
        }
    }

    public void VisibleDialogWindow()
    {
        dialogWindow.gameObject.SetActive( true );
    }

    public void UnVisibleDialogWindow()
    {
        dialogWindow.gameObject.SetActive( false );
    }

    public void SetEnabled(bool _isEnabled)
    {
        isEnabled = _isEnabled;
    }
}
