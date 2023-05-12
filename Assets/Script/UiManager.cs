using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour, IListener
{
    public GameObject GuideButton;

    [SerializeField] private GameObject statisticsImage;
    [SerializeField] private GameObject dominantImage;

    private HangingManager hangingManager;
    private bool isPossibleActiveGuide = false;
    private bool isPossibleDeactiveGuide = false;

    [SerializeField] private Canvas _screenCanvas;

    private void Awake()
    {
        GuideButton.SetActive(false);
        hangingManager = FindObjectOfType<HangingManager>();

        Debug.Assert(statisticsImage != null, "statisticsImage를 넣으세요");
        Debug.Assert(dominantImage != null, "dominentImage를 넣으세요");
        Debug.Assert(_screenCanvas != null, "screenCanvas 넣으세요");
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

    public void showDominantImage()
    {
        Destroy(FindObjectOfType<HangingTimer>().gameObject);
        dominantImage.gameObject.SetActive(true);
    }

    public void showStatistics()
    {
        statisticsImage.SetActive(true);
        FindObjectOfType<UIStatistics>().showStatistics(hangingManager.getHangingInfo());
    }

    public void showScreenCanvas()
    {
        _screenCanvas.enabled = true;
    }

    public void hideScreenCanvas()
    {
        _screenCanvas.enabled = false;
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
