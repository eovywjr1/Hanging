using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogWindowController : MonoBehaviour
{
    [SerializeField] GameObject dialogWindow;

    public void VisibleDialogWindow()
    {
        dialogWindow.gameObject.SetActive( true );
    }

    public void UnVisibleDialogWindow()
    {
        dialogWindow.gameObject.SetActive( false );
    }
}
