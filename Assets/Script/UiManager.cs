using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour, IListener
{
    public GameObject GuideButton;
    private bool isPossibleActiveGuide = false;
    private bool isPossibleDeactiveGuide = false;

    private void Awake()
    {
        GuideButton.SetActive(false);
    }

    private void Start()
    {
        EventManager.instance.addListener("possibleactiveGuide", this);
        EventManager.instance.addListener("possibledeactiveGuide", this);
    }

    private void Update()
    {
        guideOn();
    }

    private void guideOn()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if ((GuideButton.activeSelf == false) && (isPossibleActiveGuide))
            {
                GuideButton.SetActive(true);
                EventManager.instance.postNotification("dialogEvent", this, "activeGuide");

            }
            else if (GuideButton.activeSelf && isPossibleDeactiveGuide)
            {
                GuideButton.SetActive(false);
                EventManager.instance.postNotification("dialogEvent", this, "deactiveGuide");
            }
        }
    }

    public void OnEvent(string eventType, Component sender, object parameter = null)
    {
        switch (eventType)
        {
            case "possibleactiveGuide":
                isPossibleActiveGuide = true;
                break;

            case "possibledeactiveGuide":
                isPossibleDeactiveGuide = true;
                break;
        }
    }
}
