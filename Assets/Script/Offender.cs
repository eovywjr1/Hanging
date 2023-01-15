using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEditor.Tilemaps;

public class Offender : MonoBehaviour
{
    private Vector3 preMousePosition;
    [SerializeField] private bool isPossibleTodesstrafe; // 구현 완료 후 serial 삭제
    private bool isCreateLine;
    private bool isDescend;
    private bool isTodesstrafe;
    private float descendSpeed = 2f;
    private float initialMouseX;
    private float cutDistance = 100f;
    [SerializeField] private float minY; // 구현 완료 후 serial 삭제
    private RectTransform windowRectTransform;
    private HangingManager hangingManager;
    private LineManager lineManager;
    private Line line;
    private GameObject window;
    [SerializeField] private GameObject criteria;
    private Coroutine preChangeTransparency = null;
    public static OffenderData offenderData;
    
    private void Awake()
    {
        hangingManager = FindObjectOfType<HangingManager>();
        lineManager = FindObjectOfType<LineManager>();
        isPossibleTodesstrafe = true;
    }

    private void Start()
    {
        offenderData = new OffenderData();
    }

    private void Update()
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

                line.MoveTo(windowRectTransform.position.x, windowRectTransform.position.y, transform.position.y);
            }
        }
    }

    private void OnMouseDown()
    {
        if (isPossibleTodesstrafe)
        {
            if (!isCreateLine) lineManager.CreateLine(transform);
            else criteria.SetActive(true);

            isDescend = false;

            Vector3 mousePosition = new Vector3(0, Input.mousePosition.y, 0);
            initialMouseX = Input.mousePosition.x;
            preMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);   //이상한 위치로 이동 방지하기 위해 preMousePosition 초기화

            LineChangeTransparency(-1);
        }
    }

    private void OnMouseDrag()
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

            line.MoveTo(windowRectTransform.position.x, windowRectTransform.position.y, transform.position.y);
        }
    }

    private void OnMouseUp()
    {
        if (isPossibleTodesstrafe)
        {
            criteria.SetActive(false);

            LineChangeTransparency(1);

            if (transform.position.y > minY) isDescend = true;
            isCreateLine = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
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