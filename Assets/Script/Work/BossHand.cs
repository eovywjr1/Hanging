using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHand : MonoBehaviour, IListener
{
    public static BossHand instance;

    public GameObject button;

    private Vector3 originalLoca;
    private Vector3 destination;

    private bool isHoldOut;
    public bool isSubmit;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        EventManager.instance.addListener("badge", this);
        EventManager.instance.addListener("unSubmitBadgeForTwoSec", this);
        EventManager.instance.addListener("badgeLastCountdown", this);
        
        originalLoca = transform.position;
        destination = new Vector3(3, transform.position.y, transform.position.z);
        isHoldOut = false;
        isSubmit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //BossHand와 badge 충돌 시 배지 회수 동작
        isSubmit = true;
        Invoke("takeaBadge", 1.5f);

        //badge를 BossHand의 자식으로
        GameObject badge = GameObject.FindGameObjectWithTag("badge");
        if (badge != null)
            badge.transform.parent = this.gameObject.transform;
    }

    public void holdOutHand() //상사 손 내밀기
    {
        isSubmit = false;

        if (isHoldOut == false)
        {
            StartCoroutine(MoveTo(gameObject, destination));
            StartCoroutine(badgeCount(10f));
            isHoldOut = true;
        }
    }

    public void takeaBadge() //배지 회수
    {
        StartCoroutine(MoveTo(gameObject, originalLoca));
        isHoldOut = false;
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

    IEnumerator badgeCount(float time, bool _last = false)
    {
        yield return new WaitForSecondsRealtime(time);

        if (isSubmit == false)
        {
            if (time == 10f)
                EventManager.instance.postNotification("dialogEvent", this, UnityEngine.Random.Range(58, 60));
            else if (time == 2f)
                EventManager.instance.postNotification("dialogEvent", this, 60);
        }

        if (_last)
        {
            if(isSubmit)
                EventManager.instance.postNotification("dialogEvent", this, 61);
            else
                EventManager.instance.postNotification("dialogEvent", this, 62);
        }
    }

    public void OnEvent(string eventType, Component sender, object parameter = null)
    {
        switch (eventType)
        {
            case "badge":
                holdOutHand();
                break;

            case "unSubmitBadgeForTwoSec":
                badgeCount(2f);
                break;

            case "badgeLastCountdown":
                badgeCount(2f, true);
                break;
        }
    }
}
