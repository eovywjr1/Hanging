using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUiWindow : MonoBehaviour, IDragHandler
{
    [SerializeField] private GameObject clickArea; //어떤 부분을 눌러 움직여야 UI 움직이게 할 것인지
    [SerializeField] private GameObject uiObject;
    [SerializeField] private RectTransform movementArea;    //UI창이 움직일 수 있는 영역
    private RectTransform rectTransform;
    private Canvas canvas;


    private void Awake()
    {
        clickArea = this.gameObject;
        uiObject = transform.parent.gameObject;

        rectTransform = uiObject.GetComponent<RectTransform>();
        canvas = uiObject.GetComponentInParent<Canvas>();
        movementArea = rectTransform.parent.GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        rectTransform.anchoredPosition = ClampPosition(rectTransform.anchoredPosition, movementArea.rect);
    }


    private Vector2 ClampPosition(Vector2 position, Rect area)
    {
        position.x = Mathf.Clamp(position.x, area.xMin, area.xMax - rectTransform.rect.width);
        position.y = Mathf.Clamp(position.y, area.yMin, area.yMax - rectTransform.rect.height);
        
        return position;
    }
}
