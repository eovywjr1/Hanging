using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUiWindow : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private GameObject dragArea; //어떤 부분을 눌러 움직여야 UI 움직이게 할 것인지
    [SerializeField]
    private GameObject uiObject;
    private RectTransform rectTransform;
    private Canvas canvas;


    private void Awake()
    {
        dragArea = this.gameObject;
        uiObject = transform.parent.gameObject;
        rectTransform = uiObject.GetComponent<RectTransform>();
        canvas = uiObject.GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

}
