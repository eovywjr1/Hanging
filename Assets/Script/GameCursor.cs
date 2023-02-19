using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCursor : MonoBehaviour
{
    Texture2D original;
    [SerializeField]Texture2D changed;

    /*Vector3 m_vecMouseDownPos;*/
    private bool penCursor;

    void Start()
    {
        original = Resources.Load<Texture2D>("OriginalCursor");
        penCursor = false;
    }

    void Update()
    {
/*        if (Input.GetMouseButtonDown(0))

            if (Input.touchCount > 0)
            {
                m_vecMouseDownPos = Input.mousePosition;
                m_vecMouseDownPos = Input.GetTouch(0).position;
                if (Input.GetTouch(0).phase != TouchPhase.Began)
                    return;

                Vector2 pos = Camera.main.ScreenToWorldPoint(m_vecMouseDownPos);

                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name);

                    if (hit.collider.name == "Button")
                        Debug.Log("Button");
                }

            }*/

        if(Input.GetMouseButtonDown(0))
        {
            Vector2 Pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(Pos, Vector2.zero, 0f);

            if(hit.collider != null)
            {
                GameObject clickButton = hit.transform.gameObject;
                Debug.Log(clickButton.name);
            }
        }
    }

        public void OnMouseDown()
    {
        if (penCursor && Input.GetMouseButtonDown(0))
        {
            penCursor = true;
        }
        else if (!penCursor && Input.GetMouseButtonDown(0))
        {
            penCursor = false;
        }
    }

    private void OnMouseOver()
    {
        if(penCursor)
        {
            Cursor.SetCursor(changed, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(original, Vector2.zero, CursorMode.ForceSoftware);
    }
}
