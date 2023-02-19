using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowSetSize : MonoBehaviour
{
    private RectTransform rectTransform, buttonRectTransform;
    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        buttonRectTransform = transform.GetChild(1).GetComponent<RectTransform>();
    }

    public void SetSize(float x)
    {
        Vector2 vector2 = new Vector2(x, rectTransform.rect.height);

        rectTransform.sizeDelta = vector2;
        buttonRectTransform.sizeDelta = new Vector2(vector2.x, buttonRectTransform.sizeDelta.y);
        boxCollider2D.size = vector2;
    }
}
