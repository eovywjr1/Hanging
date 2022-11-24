using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMouseMove : MonoBehaviour
{
    private Vector3 preMousePosition;
    private LineToBox preLine;

    private void OnMouseDown()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        preMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);   //이상한 위치로 이동 방지하기 위해 preMousePosition 초기화
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = new Vector3(currentMousePosition.x, currentMousePosition.y, 0);
        preMousePosition = currentMousePosition;

        preLine.MoveToBox(currentMousePosition.x, currentMousePosition.y);
    }

    public void SetpreLine(LineToBox _preLine)
    {
        preLine = _preLine;
    }
}