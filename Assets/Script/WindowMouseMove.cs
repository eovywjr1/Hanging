 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowMouseMove : MonoBehaviour
{
    private Vector3 preMousePosition;
    private Line line;
    private RectTransform rectTransform;
    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        line = Line.lineList[0];
    }

    private void OnMouseDown()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        preMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);   //이상한 위치로 이동 방지하기 위해 preMousePosition 초기화
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 toPosition = currentMousePosition - preMousePosition;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + toPosition, Time.deltaTime * 10000f);
        preMousePosition = currentMousePosition;

        line.MoveTo(transform.position.x, transform.position.y);
    }

    public void SetSize(Vector2 vector2)
    {
        rectTransform.sizeDelta = vector2;
        boxCollider2D.size = vector2;
    }
}