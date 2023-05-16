using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[AddComponentMenu("UI/DebugTextComponentName", 11)]
public class GuideText : MonoBehaviour
{
    [SerializeField]
    private int day;
    public GuideDBBase guideDBBase;

    public TMP_FontAsset font;
    public int BasicFontSize;
    public int width;
    public int height;
    public Vector2 vector2;

    public GameObject scrollViewContent;
    public Button buttonPrefab;

    private GuideButton guideButtonSC;

    public List<GameObject> guideObjectList;

    private void Awake()
    {
        day = HangingManager.day;

        guideButtonSC = GetComponent<GuideButton>();

        guideDBBase = GetGuideDB();
    }

    void Start()
    {
        Debug.Log("GuideDBBase의 Entities Count!! = " + guideDBBase.Entities.Count);
        for (int i = 0; i < guideDBBase.Entities.Count; i++)
        {
            if (guideDBBase.Entities[i].day == day)
            {
                //Debug.Log("day : " + guideDBBase.Entities[i].day + ", type : " + guideDBBase.Entities[i].type + ", " + guideDBBase.Entities[i].contents);
                if (guideDBBase.Entities[i].type == "목차" || guideDBBase.Entities[i].type == "소목차"
                    || guideDBBase.Entities[i].type == "소소목차")
                {
                    createButton(guideDBBase.Entities[i]);
                }
                else if(guideDBBase.Entities[i].type == "제목")
                {
                    createGuideText(guideDBBase.Entities[i], BasicFontSize, true, Color.black);
                }
                else
                {
                    createGuideText(guideDBBase.Entities[i], BasicFontSize, false, Color.gray);
                }
            }
        }
    }

    private void Update()
    {
    }

    public GuideDBBase GetGuideDB()
    {
        return Resources.Load<GuideDB_day1>("GuideResources/GuideDB_day" + day);
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
}
