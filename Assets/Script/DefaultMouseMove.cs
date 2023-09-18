using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DefaultMouseMove : MonoBehaviour
{
    protected bool checkTopMouseClick()
    {
        Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

        if (hit.collider)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return false;

            if (hit.collider.gameObject != transform.gameObject)
                return false;
        }

        return true;
    }
}
