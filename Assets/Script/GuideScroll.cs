using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuideScroll : MonoBehaviour
{
    public GameObject content;
    public GameObject scrollContent;
    public GameObject scrollViewBar;
    Scrollbar scrollbar;
    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = scrollContent.GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectTransform.sizeDelta = new Vector2(500, scrollContent.transform.childCount * 100);
        if(scrollViewBar.activeSelf)
        {
            scrollbar = scrollViewBar.GetComponent<Scrollbar>();
            if(scrollbar.value < 0)
            {
                scrollbar.value = 0.1f;
                createContent();
            }
        }
    }

    //가이드창 ON/OFF
    private void guideOnOff()
    {

    }

    private void createLabel()
    {

    }

    //가이드 창 안의 Content 생성
    private void createContent()
    {

    }

    //스크롤바 맨 위로 올리기
    public void upToTop()
    {

    }

}
