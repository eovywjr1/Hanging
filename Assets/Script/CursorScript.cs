using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    Texture2D original;
    [SerializeField] Texture2D changed;

    public bool penCursor;

    void Start()
    {
        original = Resources.Load<Texture2D>("OriginalCursor");
        penCursor = false;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if(hit.collider != null)
            {
                GameObject clickObj = hit.transform.gameObject;
                Debug.Log(clickObj.name);
            }

            if(hit.transform.gameObject.name == "penButton")
            {
                if (!penCursor)
                    penCursor = true;
                else
                    penCursor = false;
            }
        }
    }

    private void OnMouseEnter()
    {
        if (penCursor)
        {
            Cursor.SetCursor(changed, Vector2.zero, CursorMode.Auto);
        }
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(original, Vector2.zero, CursorMode.Auto);
    }
}
