using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowSetSize : MonoBehaviour
{
    private RectTransform rectTransform, buttonRectTransform, dataRectTransform;
    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        buttonRectTransform = transform.GetChild(1).GetComponent<RectTransform>();
        dataRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void SetSize(float x)
    {
        Vector2 vector2 = new Vector2(x, rectTransform.rect.height);

        rectTransform.sizeDelta = vector2;
        buttonRectTransform.sizeDelta = new Vector2(vector2.x - 0.7f, buttonRectTransform.sizeDelta.y);
        dataRectTransform.sizeDelta = new Vector2(vector2.x - 0.7f, dataRectTransform.sizeDelta.y);
        boxCollider2D.size = vector2;
    }
}
