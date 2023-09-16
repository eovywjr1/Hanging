using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hanging;

public class UiManager : MonoBehaviour, IListener
{
    private HangingManager hangingManager;

    private GameObject GuideWindow;
    [SerializeField] private GameObject statisticsImage;
    [SerializeField] private GameObject dominantImage;

    private bool isPossibleActiveGuide = false;
    private bool isPossibleDeactiveGuide = false;

    [SerializeField] private Canvas _screenCanvas;

    private void Awake()
    {
        GuideWindow = GameObject.Find("CctvCanvas").transform.Find("GuideScrollView").gameObject;
        Debug.Assert(GuideWindow != null, "GuideScrollView인 Object가 없습니다.");

        GuideWindow.SetActive(false);
        hangingManager = FindObjectOfType<HangingManager>();

        Debug.Assert(statisticsImage != null, "statisticsImage가 없습니다.");
        Debug.Assert(dominantImage != null, "dominentImage가 없습니다.");
        Debug.Assert(_screenCanvas != null, "screenCanvas가 없습니다.");
    }

    private void Start()
    {
        EventManager.instance.addListener("possibleactiveGuide", this);
        EventManager.instance.addListener("possibledeactiveGuide", this);
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

                    //조민수 : [삭제 예정] 각 UI들이 Raycast 사용하면 삭제해야 함//
                    Rope rope = FindObjectOfType<Rope>();
                    AttackerMouseMove attackerMouseMove = FindObjectOfType<AttackerMouseMove>();

                    if (rope)
                        rope.SetCutPossible(true);

                    if (attackerMouseMove)
                        attackerMouseMove.SetPossibleTodesstrafe(true);
                    //////////////////////////////////////////////////////////////
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
        FindObjectOfType<UIStatistics>().setStatistics(hangingManager.getHangingInfo());
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
