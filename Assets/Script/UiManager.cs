using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour, IListener
{
    public GameObject GuideWindow;

    [SerializeField]
    private AttackerMouseMove attackerMouseMove;
    [SerializeField]
    private Rope rope;

    [SerializeField] private GameObject statisticsImage;
    [SerializeField] private GameObject dominantImage;

    private HangingManager hangingManager;
    private bool isPossibleActiveGuide = false;
    private bool isPossibleDeactiveGuide = false;

    [SerializeField] private Canvas _screenCanvas;

    private void Awake()
    {
        GuideWindow.SetActive(false);
        hangingManager = FindObjectOfType<HangingManager>();

        Debug.Assert(statisticsImage != null, "statisticsImage�� ��������");
        Debug.Assert(dominantImage != null, "dominentImage�� ��������");
        Debug.Assert(_screenCanvas != null, "screenCanvas ��������");
    }

    private void Start()
    {
        EventManager.instance.addListener("possibleactiveGuide", this);
        EventManager.instance.addListener("possibledeactiveGuide", this);

        rope = GameObject.Find("rope").GetComponent<Rope>();
        attackerMouseMove = GameObject.Find("Prisoner(Clone)").GetComponent<AttackerMouseMove>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if ((GuideWindow.activeSelf == false) && (isPossibleActiveGuide))
            {
                GuideWindow.SetActive(true);
                EventManager.instance.postNotification("dialogEvent", this, "activeGuide");

            }
            else if (GuideWindow.activeSelf && isPossibleDeactiveGuide)
            {
                GuideWindow.SetActive(false);
                EventManager.instance.postNotification("dialogEvent", this, "deactiveGuide");
                if (GuideWindow.activeSelf == true)
                {
                    GuideWindow.SetActive(false);
                    attackerMouseMove.SetPossibleTodesstrafe(true);
                    //Debug.Log("������ Ŭ�� Ǯ��");
                    rope.SetCutPossible(true);
                    //Debug.Log("���� �� Ǯ��");
                }
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
