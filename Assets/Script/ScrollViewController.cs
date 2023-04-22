using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Unity.Burst.CompilerServices;

public class ScrollViewController : MonoBehaviour
{
    private ScrollRect scrollRect;
    float space = 0f;

    public GameObject uiPrefab;
    public List<RectTransform> uiObjects= new List<RectTransform>();
    public AttackerInfo attackerInfo;
    public TMP_Text txtALlTmp;
    public TMP_Text forWidth;
    private bool compareMentBool;
    private bool flag;
    private float height;
    // Start is called before the first frame update
    void Start()
    {
        attackerInfo = FindObjectOfType<AttackerInfo>();
        scrollRect = GetComponent<ScrollRect>();
        flag = false;
        //txtALlTmp=new TMP_Text();
        //txtALlTmp = GetComponent<TMP_Text>();
        //forWidth= GetComponent<TMP_Text>(); 
        //ShowMent();

    }
    private void Update()
    {
        
        if (flag == false)
        {
            string[] fixtext = { "이름 : ", "범죄 : ", "범행장소 : ", "범행동기 : " };
            for (int i = 0; i < 4; i++)
            {
                compareMentBool = attackerInfo.recordData.correctState[i].Equals(attackerInfo.recordData.currentState[i]) ? true : false;
                AddMent(fixtext[i] + attackerInfo.recordData.currentState[i], compareMentBool, attackerInfo.recordData.lieORinfoErrorValue);
            }
            flag = true;
            //MakeMentList();
            
        }
       
    }
    public void AddMent(string str, bool compareMentBool, int lieORinfoErrorValue)
    {
        var newUi=Instantiate(uiPrefab,scrollRect.content).GetComponent<RectTransform>();
        //str = "안녕하세요안녕하세요안녕하세요안녕하세요안녕하세요안녕하세요안녕하세요안녕하세요안녕하세요안녕하세요";
        string txtAll =str;
        int lineCnt = 1;
        txtALlTmp.text = txtAll;
        string resultStr="";

        Debug.Log("이 정보가 틀리냐 맞냐!!!! : "+compareMentBool);

        //대화창 가로폭보다 긴 텍스트는 줄내림//
        if (txtALlTmp.preferredWidth >= 300)  
        {
            (resultStr, txtAll, lineCnt) =LineCut(str);
            
            if (compareMentBool == false)
            {
                newUi.GetComponent<ChangeTextTexture>().lastMent = txtAll;
                Debug.Log("마지막문장:"+txtAll);
            }
        }
        else//대화창 가로폭보다 짧은 텍스트는 그대로 출력//
        {
            resultStr = str;

            if (compareMentBool == false)
            {
                newUi.GetComponent<ChangeTextTexture>().lastMent = resultStr;
                Debug.Log("마지막문장:" + resultStr);
            }
        }

        newUi.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = resultStr;
        newUi.GetComponent<ChangeTextTexture>().mentTureORFalse = compareMentBool;
        newUi.GetComponent<ChangeTextTexture>().lieORinfoErrorValue = lieORinfoErrorValue;
        uiObjects.Add(newUi);

        LineSpacing(lineCnt);
    }

    public void AddChangedMent(string str, bool compareMentBool, int lieORinfoErrorValue, bool currentClick)
    {
        var newUi = Instantiate(uiPrefab, scrollRect.content).GetComponent<RectTransform>();
        string resultStr = str;

        newUi.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = resultStr;
        newUi.GetComponent<ChangeTextTexture>().mentTureORFalse = compareMentBool;
        newUi.GetComponent<ChangeTextTexture>().afterClick = currentClick;
        newUi.GetComponent<ChangeTextTexture>().lieORinfoErrorValue = lieORinfoErrorValue;
        uiObjects.Add(newUi);

        //위증 또는 정보오류 글씨 색 변경
        if(currentClick==true && compareMentBool == false)
        {
            //newUi.transform.GetChild(0).GetComponent<Image>().color = new Color32(217, 66, 66, 255); //빨간배경
            newUi.transform.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().color = new Color32(217, 66, 66, 255);
        }
        //잘못 클릭한 올바른 정보 글씨 색 변경
        else if (currentClick == true && compareMentBool == true)
        {
            newUi.transform.GetChild(0).GetComponent<Image>().color = new Color32(239, 239, 239, 255); //회색배경
        }

        LineSpacing(Regex.Matches(str,"\n").Count+1);

        Debug.Log("클릭후? :" + newUi.GetComponent<ChangeTextTexture>().afterClick);
    }

    public void MakeMentList()
    {
        string[] fixtext = { "이름 : ", "죄목 : ", "발생장소 : ", "경위 : " };
        height = 5f;
        for (int i = 0; i < 4; i++)
        {
            compareMentBool = attackerInfo.recordData.correctState[i].Equals(attackerInfo.recordData.currentState[i]) ? true : false;
            AddMent(fixtext[i] + attackerInfo.recordData.currentState[i], compareMentBool, attackerInfo.recordData.lieORinfoErrorValue);
        }
        flag = true;
    }

    public void MakeMentCangedList(List<string> currentList, List<bool> currentClick)
    {
        string[] fixtext = { "이름 : ", "죄목 : ", "발생장소 : ", "경위 : " };
        height = 5f;
        for (int i = 0; i < 4; i++)
        {
            compareMentBool = attackerInfo.recordData.correctState[i].Equals(attackerInfo.recordData.currentState[i]) ? true : false;
            AddChangedMent(currentList[i], compareMentBool, attackerInfo.recordData.lieORinfoErrorValue, currentClick[i]);
        }
        flag = true;
    }

    (string,string,int) LineCut(string str)
    {
        string txtAll = str;
        string resultStr = "";
        string addStr = "";
        int lineCnt=1;

        while (txtALlTmp.preferredWidth > 300)
        {
            //초기화
            addStr = "";
            forWidth.text = "";

            for (int i = 0; forWidth.preferredWidth <= 300; i++)
            {
                addStr = addStr + txtAll[0];
                txtAll = txtAll.Remove(0, 1);
                txtALlTmp.text = txtAll;

                forWidth.text = addStr; // addStr 길이를 알기 위함

            }
            lineCnt++;
            resultStr = resultStr + addStr + System.Environment.NewLine; //한줄 내림

        }

        resultStr = resultStr + txtAll;

        return (resultStr, txtAll,lineCnt);
    }

    void LineSpacing(float lineCnt)
    {
        uiObjects[uiObjects.Count - 1].anchoredPosition = new Vector2(0f, -height);
        height += 10 * lineCnt + space; //9*lineCnt
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, height);
    } 
}
