using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingMove : MonoBehaviour
{
    private Vector3 preMousePosition;
    [SerializeField] private GameObject criteria;
    [SerializeField] private bool isPossibleTodesstrafe; // 구현 완료 후 serial 삭제
    private bool isDescend;
    private float descendSpeed = 2f;
    private float initialMouseX;
    private float cutDistance = 100f;
    [SerializeField] private float minY; // 구현 완료 후 serial 삭제
    private HangingManager hangingManager;

    private void Start()
    {
        hangingManager = FindObjectOfType<HangingManager>();
    }

    private void Update()
    {
        if (isDescend)
        {
            transform.Translate(new Vector3(0, -1 * descendSpeed * Time.deltaTime));
            if (transform.position.y <= minY)
                isDescend = false;
        }
    }

    private void OnMouseDown()
    {
        criteria.SetActive(true);

        Vector3 mousePosition = new Vector3(0, Input.mousePosition.y, 0);
        initialMouseX = Input.mousePosition.x;
        preMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);   //이상한 위치로 이동 방지하기 위해 preMousePosition 초기화
    }

    private void OnMouseDrag()
    {
        if (isPossibleTodesstrafe)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            float nextY = currentMousePosition.y - preMousePosition.y;

            if (Mathf.Abs(mousePosition.x - initialMouseX) >= cutDistance)
                hangingManager.Todesstrafe();

            if (transform.position.y > minY || nextY > 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + nextY, 0);
                preMousePosition = currentMousePosition;
            }
        }
    }

    private void OnMouseUp()
    {
        criteria.SetActive(false);

        isDescend = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPossibleTodesstrafe)
        {
            if (collision.CompareTag("criteria"))
                hangingManager.Todesstrafe();
        }
    }

    public void SetisPossibleTodesstrafe(bool _isPossibleTodesstrafe)
    {
        isPossibleTodesstrafe = _isPossibleTodesstrafe;
    }
}
