using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowSetSize : MonoBehaviour
{
    private RectTransform rectTransform;
    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void SetSize(float x)
    {
        Vector2 vector2 = new Vector2(x, rectTransform.rect.height);

        rectTransform.sizeDelta = vector2;
        boxCollider2D.size = vector2;
    }
}
