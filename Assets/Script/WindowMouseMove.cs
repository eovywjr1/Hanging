 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WindowMouseMove : MonoBehaviour
{
    private Vector3 preMousePosition;
    private Line line;
    private int lineIdx;
    private HallucinogenicEffect hallucinogenicEffect;

    private void Start()
    {
        hallucinogenicEffect = FindObjectOfType<HallucinogenicEffect>();
        for (int i = 0; i < Line.lineList.Count; i++)
        {
            if(Line.lineList[i].windowObject == this.gameObject)
            {
                line = Line.lineList[i];
                lineIdx = i;
                break;
            }
        }

        //int x = Random.Range(0.25f, 0.75f);
        //viewPos.x = Mathf.Clamp(viewPos.x, 0.25f, 0.75f);
        //viewPos.y = Mathf.Clamp(viewPos.y, 0.25f, 0.75f);


    }

    private void OnMouseDown()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        preMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);   //이상한 위치로 이동 방지하기 위해 preMousePosition 초기화
    }

    private void OnMouseDrag()
    {
        //window 이동//
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);   
        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 toPosition = currentMousePosition - preMousePosition;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + toPosition, Time.deltaTime * 10000f);
        preMousePosition = currentMousePosition;

        //CCTV 레이어 범위 제한//
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        viewPos.x = Mathf.Clamp(viewPos.x, 0.25f, 0.75f);
        viewPos.y = Mathf.Clamp(viewPos.y, 0.25f, 0.75f);
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        transform.position = worldPos;

        //1개 혹은 2개 선 업데이트//
        if (lineIdx == 0)
        {
            line.MoveTo(transform.position.x, transform.position.y);

            if (Line.lineList.Count > 1)
            {
                Line nextLine = Line.lineList[lineIdx + 1];
                Transform nextWindowTransform = nextLine.windowObject.transform;

                nextLine.MoveTo(nextWindowTransform.position.x, nextWindowTransform.position.y, transform.position.x, transform.position.y);
            }
        }
        else
        {
            Transform preWindowTransform = Line.lineList[lineIdx - 1].windowObject.transform;

            line.MoveTo(transform.position.x, transform.position.y, preWindowTransform.position.x, preWindowTransform.position.y);
        }
    }
}