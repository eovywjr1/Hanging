using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePointerAtMent : MonoBehaviour
{
    [SerializeField] Texture2D changed;
    private CursorScript cursorScript;

    private void Awake()
    {
        cursorScript = FindObjectOfType<CursorScript>();
    }

    private void OnMouseEnter()
    {
        if (cursorScript.penCursor)
        {
            Cursor.SetCursor(changed, Vector2.zero, CursorMode.Auto);
        }

    }
}
