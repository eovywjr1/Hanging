using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogWindowController : MonoBehaviour
{
    public Canvas dialogCanvas;

    [SerializeField] GameObject dialogWindow;

    private Image dialogViewImage = null;
    private Image dialogViewportImage = null;
    private Image dialogScrollbarImage = null;
    private Image dialogScrollbarHandleImage = null;

    bool isEnabled;

    private void Awake()
    {
        Transform dialogTransform = dialogWindow.transform;
        dialogCanvas = dialogTransform.parent.GetComponent<Canvas>();
        dialogViewImage = dialogWindow.GetComponent<Image>();
        dialogViewportImage = dialogTransform.GetChild(0).GetComponent<Image>();
        dialogScrollbarImage = dialogTransform.GetChild(1).GetComponent<Image>();
        dialogScrollbarHandleImage = dialogTransform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && isEnabled)
        {
            if (dialogViewImage.color.a == 0)
                VisibleDialogWindow();
            else
                UnVisibleDialogWindow();
        }
    }

    public void VisibleDialogWindow()
    {
        dialogViewImage.color = new Color(1, 1, 1, 0.5f);
        dialogViewportImage.color = new Color(1, 1, 1, 1);
        dialogScrollbarImage.color = new Color(1, 1, 1, 1);
        dialogScrollbarHandleImage.color = new Color(1, 1, 1, 1);
    }

    public void UnVisibleDialogWindow()
    {
        dialogViewImage.color = new Color(1, 1, 1, 0);
        dialogViewportImage.color = new Color(1, 1, 1, 0);
        dialogScrollbarImage.color = new Color(1, 1, 1, 0);
        dialogScrollbarHandleImage.color = new Color(1, 1, 1, 0);
    }

    public void SetEnabled(bool _isEnabled)
    {
        isEnabled = _isEnabled;
    }
}
