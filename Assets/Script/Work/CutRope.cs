using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CutRope : MonoBehaviour, IListener
{
    private Rope ropeScript;

    Vector3 initialMousePosition;
    public GameObject rope;

    private float cutDistance = 2f;
    bool isCut, isPossibleAmnesty, isAmnesty;

    private void Awake()
    {
        ropeScript = FindObjectOfType<Rope>();

        EventManager.instance.addListener("possibleamnesty", this);
    }

    private void Update()
    {
        if (isPossibleAmnesty && isAmnesty == false)
        {
            /*if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !EventSystem.current.IsPointerOverGameObject() && rope != null)
            {
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                initialMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                isCut = true;
            }
            else if (isCut && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
            {
                Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                Vector3 ropePosition = rope.transform.position;

                if ((initialMousePosition.x >= ropePosition.x && initialMousePosition.x - currentMousePosition.x >= cutDistance && currentMousePosition.x < ropePosition.x) ||
                    (initialMousePosition.x < ropePosition.x && currentMousePosition.x - initialMousePosition.x >= cutDistance && currentMousePosition.x > ropePosition.x))
                {
                    isAmnesty = true;
                    EventManager.instance.postNotification("amnesty", this, null);
                    EventManager.instance.postNotification("dialogEvent", this, UnityEngine.Random.Range(21, 28));
                    EventManager.instance.postNotification("dialogEvent", this, "amnesty");

                }
            }
            else if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
            {
                isCut = false;
            }*/

            if(ropeScript.isCut && !EventSystem.current.IsPointerOverGameObject() && rope != null)
            {
                isAmnesty = true;

                EventManager.instance.postNotification("dialogEvent", this, UnityEngine.Random.Range(21, 28));

                StartCoroutine(DelayedEvents());
            }
        }
    }

    public void OnEvent(string eventType, Component sender, object parameter = null)
    {
        switch (eventType)
        {
            case "possibleamnesty":
                isPossibleAmnesty = true;
                break;
        }
    }

    IEnumerator DelayedEvents()
    {
        yield return new WaitForSeconds(3.1f);

        EventManager.instance.postNotification("amnesty", this, null);
        EventManager.instance.postNotification("dialogEvent", this, "amnesty");
    }
}