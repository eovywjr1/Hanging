using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogWindowController : MonoBehaviour
{
    public void VisibleDialogWindow()
    {
        gameObject.SetActive( true );
    }

    public void UnVisibleDialogWindow()
    {
        gameObject.SetActive( false );
    }
}
