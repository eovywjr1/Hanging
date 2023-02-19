using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCursor : MonoBehaviour
{
    Texture2D original;
    [SerializeField]Texture2D changed;

    void Start()
    {
        original = Resources.Load<Texture2D>("OriginalCursor");
    }

    private void OnMouseOver()
    {
        Cursor.SetCursor(changed, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(original, Vector2.zero, CursorMode.ForceSoftware);
    }
}
