using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowMouseMove : MonoBehaviour
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
        Vector3 toPosition = currentMousePosition - preMousePosition;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + toPosition, Time.deltaTime * 10000f);
        preMousePosition = currentMousePosition;

        preLine.MoveToBox(transform.position.x, transform.position.y);
    }

    public void SetpreLine(LineToBox _preLine)
    {
        preLine = _preLine;
    }
}