using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[AddComponentMenu("UI/DebugTextComponentName", 11)]
public class GuideText : MonoBehaviour
{
    public int day;
    public GuideDB guideDB;

    public TMP_FontAsset font;
    public int BasicFontSize;
    public int width;
    public int height;
    public Vector2 vector2;

    public GameObject scrollViewContent;
    public Button buttonPrefab;

    public List<GameObject> guideObjectList;

    private GuideButton guideButtonSC;
    private bool isLoaded = false;

    private void Awake()
    {
        guideButtonSC = GetComponent<GuideButton>();
    }

    private void Start()
    {
        for (int i = 0; i < guideDB.Entities.Count; i++)
        {
            if (guideDB.Entities[i].day == day)
            {
                if (guideDB.Entities[i].type == "목차" || guideDB.Entities[i].type == "소목차"
                    || guideDB.Entities[i].type == "소소목차")
                {
                    createButton(guideDB.Entities[i]);
                }
                else if(guideDB.Entities[i].type == "제목")
                {
                    createGuideText(guideDB.Entities[i], BasicFontSize, true, Color.black);
                }
                else
                {
                    createGuideText(guideDB.Entities[i], BasicFontSize, false, Color.gray);
                }
            }
        }

        isLoaded = true;
    }

    private void Update()
    {
    }

    void createGuideText(GuideDBEntity guideDB, int fontSize, bool isBold, Color fontColor)
    {
        GameObject guideText;
        if (guideDB.type == "제목")
            guideText = new GameObject("guide" + guideDB.number); //제목에 해당하는 것은 목차와 연결할 번호로 이름 지정
        else
            guideText = new GameObject("guideText");

        guideText.transform.SetParent(scrollViewContent.transform);

        //가이드내용 기본 정보
        setGuideInfo(guideText, guideDB);

        TextMeshProUGUI newTextComponent = guideText.AddComponent<TextMeshProUGUI>();

        newTextComponent.text = guideDB.contents.ToString();

        //폰트 컬러
        newTextComponent.color = fontColor;

        //폰트 설정
        newTextComponent.font = font;

        //폰트 굵게
        if(isBold)
        {
            newTextComponent.fontStyle = FontStyles.Bold;
        }

        //폰트 사이즈
        newTextComponent.fontSize = fontSize;

        //텍스트 위치, 크기 조정
        RectTransform rectTransform = newTextComponent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, height);
        rectTransform.localScale = vector2;

        //세로 길이를 컨텐트사이즈 필터로 조절
        ContentSizeFitter sizeFitter = guideText.AddComponent<ContentSizeFitter>();
        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        guideObjectList.Add(guideText);
    }

    void createButton(GuideDBEntity guideDB)
    {
        Button newButton = Instantiate(buttonPrefab);
        newButton.transform.transform.SetParent(scrollViewContent.transform);

        //가이드내용 기본 정보
        setGuideInfo(newButton.gameObject, guideDB);

        TextMeshProUGUI newTextComponent = newButton.GetComponentInChildren<TextMeshProUGUI>();

        newTextComponent.text = guideDB.contents.ToString();

        //Button의 rectTransform 조정
        RectTransform rectTransform = newButton.GetComponent<RectTransform>();
        rectTransform.localScale = vector2;

        //Text의 rectTransform 조정
        RectTransform btnRect = newTextComponent.GetComponent<RectTransform>();
        if (guideDB.type == "소목차") 
        {
            btnRect.offsetMin = new Vector2(5, 0);
        }
        else if (guideDB.type == "소소목차")
        {
            btnRect.offsetMin = new Vector2(10, 0);
        }
    }

    void setGuideInfo(GameObject guideText, GuideDBEntity guideDBEntity)
    {
        GuideTextInfo guideTextInfo = guideText.AddComponent<GuideTextInfo>();

        guideTextInfo.day = guideDBEntity.day;
        guideTextInfo.number = guideDBEntity.number;
        guideTextInfo.type = guideDBEntity.type;
    }

    public bool checkLoaded()
    {
        return isLoaded;
    }
}
