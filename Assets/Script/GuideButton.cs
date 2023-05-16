using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideButton : MonoBehaviour
{
    [SerializeField]
    private ScrollRect scrollRect;

    private void Awake()
    {
        //scrollRect = GetComponentInChildren<ScrollRect>();
        scrollRect = GameObject.Find("GuideScrollView").GetComponent<ScrollRect>();
    }

    private void OnEnable()
    {
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }

    //가이드창 닫기
    public void close()
    {
        this.gameObject.SetActive(false);
    }

    //스크롤 맨 위로 올리기
    public void upToTop()
    {
        //Debug.Log("Guide Scroll : UpToTop!! ");
        scrollRect.verticalNormalizedPosition = 1f;
    }

    //스크롤을 해당 위치로
    public void goToLoc()
    {
        //해당 버튼의 정보 가져오기
        GuideTextInfo guideTextInfo = this.GetComponent<GuideTextInfo>();

        GameObject content = GameObject.Find("guide" + guideTextInfo.number);
        Debug.Log(content.name);

        RectTransform rectTransform = content.GetComponent<RectTransform>();

        float normalizedPosition
            = rectTransform.anchoredPosition.y
            / (rectTransform.rect.height - scrollRect.content.rect.height);

        scrollRect.verticalNormalizedPosition = 1f - normalizedPosition;
    }
}

