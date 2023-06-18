using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossHand : MonoBehaviour, IListener
{
    public static BossHand instance;
    public GameObject button;

    private Vector3 originalLoca;
    private Vector3 destination;

    public bool isSubmit = false;

    private void Awake()
    {
        instance = this;

        EventManager.instance.addListener("badge", this);
    }

    private void Start()
    {
        originalLoca = transform.position;
        destination = new Vector3(3, transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("badge"))
        {
            isSubmit = true;

            collision.transform.parent = this.gameObject.transform;

            EventManager.instance.postNotification("submitBadge", this, null);

            StartCoroutine(takeaBadge(collision.gameObject));
        }
    }

    public void holdOutHand() //상사 손 내밀기
    {
        isSubmit = false;

        StartCoroutine(MoveTo(gameObject, destination));

        CameraMoveScript cameraMove = FindObjectOfType<CameraMoveScript>();
        if(cameraMove == null)
        {
            Debug.Assert(false, "조민수 : mainCamera의 CameraMoveScript가 없어서 확인부탁드립니다.");
            return;
        }
        cameraMove.moveToDesk(-1);

        DialogUpdateAndEvent dialogUpdateAndEvent = FindObjectOfType<DialogUpdateAndEvent>();
        if(dialogUpdateAndEvent == null)
        {
            Debug.Assert(false, "조민수 : dialogScrollView의 content에 DialogUpdateAndEvent가 없어서 확인부탁드립니다.");
            return;
        }
        StartCoroutine(dialogUpdateAndEvent.dialogUnsubmitBadge(10));
    }

    private IEnumerator takeaBadge(GameObject badge) //배지 회수
    {
        yield return new WaitForSecondsRealtime(1.5f);

        yield return StartCoroutine(backHandAndDestroyBadge(badge));
    }

    IEnumerator MoveTo(GameObject obj, Vector3 toPos)
    {
        float cnt = 0;
        Vector3 nowPos = obj.transform.position;

        while(true)
        {
            cnt += Time.deltaTime;
            obj.transform.position = Vector3.Lerp(nowPos, toPos, cnt);

            if(cnt >= 1)
            {
                obj.transform.position = toPos;
                break;
            }
            yield return null;
        }
    }

    private IEnumerator backHandAndDestroyBadge(GameObject badge)
    {
        yield return StartCoroutine(MoveTo(gameObject, originalLoca));

        Destroy(badge);
    }

    public IEnumerator checkSubmit(float time, Action<bool> callback)
    {
        yield return new WaitForSecondsRealtime(time);

        callback(isSubmit);
    }

    public void OnEvent(string eventType, Component sender, object parameter = null)
    {
        switch (eventType)
        {
            case "badge":
                holdOutHand();
                break;
        }
    }
}
