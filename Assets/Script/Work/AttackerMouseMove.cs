using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using UnityEditor.Tilemaps;

public class AttackerMouseMove : MonoBehaviour, IListener
{
    HangingManager hangingManager;
    LineManager lineManager;

    Vector3 preMousePosition;
    [SerializeField]
    bool isPossibleTodesstrafe, isPossibleClick, isCreateLine, isDescend, isFirstClick = true;
    float descendSpeed = 2f;
    float initialMouseX;
    [SerializeField] float minY; // ���� �Ϸ� �� serial ����
    RectTransform windowRectTransform;
    Line line;
    GameObject window;
    [SerializeField] GameObject criteria;
    Coroutine preChangeTransparency = null;

    prisoner prisoner;

    void Awake()
    {
        hangingManager = FindObjectOfType<HangingManager>();
        lineManager = FindObjectOfType<LineManager>();
    }

    private void Start()
    {
        EventManager.instance.addListener("possibleclickAttacker", this);
        EventManager.instance.addListener("possibletodesstrafe", this);
        isPossibleTodesstrafe = true;

        prisoner = GetComponentInChildren<prisoner>();
    }

    void Update()
    {
        if (isPossibleTodesstrafe) 
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

                if (transform == null || windowRectTransform == null)
                    return;
                line.MoveTo(windowRectTransform.position.x, windowRectTransform.position.y, transform.position.x, transform.position.y);
            }
        }
    }

    void OnMouseDown()
    {
        if (isPossibleClick || isPossibleTodesstrafe)
        {
            EventManager.instance.postNotification("dialogEvent", this, "clickAttacker");

            if (isCreateLine == false)
            {
                lineManager.CreateLine();
                isCreateLine = true;
            }
            else
            {
                LineChangeTransparency(-1);
            }

            isDescend = false;

            Vector3 mousePosition = new Vector3(0, Input.mousePosition.y, 0);
            initialMouseX = Input.mousePosition.x;
            preMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);   //�̻��� ��ġ�� �̵� �����ϱ� ���� preMousePosition �ʱ�ȭ

            LineChangeTransparency(-1);

            prisoner.isLift = true;
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

            if (transform == null || windowRectTransform == null)
                return;
            line.MoveTo(windowRectTransform.position.x, windowRectTransform.position.y, transform.position.x, transform.position.y);

            prisoner.isLift = true;
        }
    }

    void OnMouseUp()
    {
        if (isPossibleTodesstrafe)
        {
            if (isFirstClick == false)
                LineChangeTransparency(1);

            if (transform.position.y > minY)
                isDescend = true;

            isFirstClick = false;

            prisoner.isLift = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPossibleTodesstrafe)
        {
            if (collision.CompareTag("TodesstrafeCriteria"))
            {
                isPossibleTodesstrafe = false;
                EventManager.instance.postNotification("todesstrafe", this, null);
                EventManager.instance.postNotification("dialogEvent", this, "todesstrafe");
            }

            if ((collision.CompareTag("middleCriteria")) && (isDescend == false))
                EventManager.instance.postNotification("dialogEvent", this, UnityEngine.Random.Range(11, 21));
        }
    }

    public void SetPossibleTodesstrafe(bool _isPossibleTodesstrafe)
    {
        isPossibleTodesstrafe = _isPossibleTodesstrafe;
    }

    public void LineChangeTransparency(int mode)
    {
        if (preChangeTransparency != null) StopCoroutine(preChangeTransparency);
        if (line != null) preChangeTransparency = StartCoroutine(line.ChangeTransparency(mode));
    }

    public void OnEvent(string eventType, Component sender, object parameter = null)
    {
        switch (eventType)
        {
            case "possibleclickAttacker":
                isPossibleClick = true;
                break;

            case "possibletodesstrafe":
                isPossibleTodesstrafe = true;
                break;
        }
    }
}