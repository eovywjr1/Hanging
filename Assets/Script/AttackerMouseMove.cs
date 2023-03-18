using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEditor.Tilemaps;

public class AttackerMouseMove : MonoBehaviour
{
    Vector3 preMousePosition;
    [SerializeField] bool isPossibleTodesstrafe; // 구현 완료 후 serial 삭제
    bool isCreateLine;
    bool isDescend;
    bool isTodesstrafe;
    float descendSpeed = 2f;
    float initialMouseX;
    [SerializeField] float minY; // 구현 완료 후 serial 삭제
    RectTransform windowRectTransform;
    HangingManager hangingManager;
    LineManager lineManager;
    Line line;
    GameObject window;
    [SerializeField] GameObject criteria;
    Coroutine preChangeTransparency = null;

    void Awake()
    {
        hangingManager = FindObjectOfType<HangingManager>();
        lineManager = FindObjectOfType<LineManager>();
        isPossibleTodesstrafe = true;
    }

    void Update()
    {
        if (isPossibleTodesstrafe || !isTodesstrafe)
        {
            if (Line.lineList.Count > 0 && line == null)
            {
                line = Line.lineList[0];
                window = Line.lineList[0].windowObject;
                windowRectTransform = window.GetComponent<RectTransform>();
            }

            if (isDescend)
            {
                transform.Translate(new Vector3(0, -1 * descendSpeed * Time.deltaTime));
                if (transform.position.y <= minY)
                    isDescend = false;

                line.MoveTo(windowRectTransform.position.x, windowRectTransform.position.y, transform.position.x, transform.position.y);
            }
        }
    }

    void OnMouseDown()
    {
        if (isPossibleTodesstrafe)
        {
            if (!isCreateLine) lineManager.CreateLine();
            else criteria.SetActive(true);

            isDescend = false;

            Vector3 mousePosition = new Vector3(0, Input.mousePosition.y, 0);
            initialMouseX = Input.mousePosition.x;
            preMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);   //이상한 위치로 이동 방지하기 위해 preMousePosition 초기화

            LineChangeTransparency(-1);
        }
    }

    void OnMouseDrag()
    {
        if (isPossibleTodesstrafe && isCreateLine)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            float nextY = currentMousePosition.y - preMousePosition.y;

            if (transform.position.y > minY || nextY > 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + nextY, 0);
                preMousePosition = currentMousePosition;
            }

            line.MoveTo(windowRectTransform.position.x, windowRectTransform.position.y, transform.position.x, transform.position.y);
        }
    }

    void OnMouseUp()
    {
        if (isPossibleTodesstrafe)
        {
            criteria.SetActive(false);

            if(isCreateLine) LineChangeTransparency(1);

            if (transform.position.y > minY) isDescend = true;
            isCreateLine = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPossibleTodesstrafe)
        {
            if (collision.CompareTag("criteria"))
            {
                hangingManager.Todesstrafe();
                isPossibleTodesstrafe = false;
                isTodesstrafe = true;
            }
        }
    }

    public void SetisPossibleTodesstrafe(bool _isPossibleTodesstrafe)
    {
        isPossibleTodesstrafe = _isPossibleTodesstrafe;
    }

    public void LineChangeTransparency(int mode)
    {
        if (preChangeTransparency != null) StopCoroutine(preChangeTransparency);
        if (line != null) preChangeTransparency = StartCoroutine(line.ChangeTransparency(mode));
    }
}