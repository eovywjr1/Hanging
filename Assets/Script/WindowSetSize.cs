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
        if(transform.childCount > 2) buttonRectTransform = transform.GetChild(2).GetComponent<RectTransform>();
        dataRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void SetSize(float x)
    {
        Vector2 vector2 = new Vector2(x, rectTransform.rect.height);

        rectTransform.sizeDelta = vector2;
        if (buttonRectTransform != null) buttonRectTransform.sizeDelta = new Vector2(vector2.x - 0.75f, buttonRectTransform.sizeDelta.y);
        dataRectTransform.sizeDelta = new Vector2(vector2.x - 0.75f, dataRectTransform.sizeDelta.y);
        if(boxCollider2D != null) boxCollider2D.size = new Vector2(x, 1);
    }
}