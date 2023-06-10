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
    public GameObject content;
    private bool compareMentBool;
    private float height;
    // Start is called before the first frame update
    void Start()
    {
        attackerInfo = FindObjectOfType<AttackerInfo>();
        scrollRect = GetComponent<ScrollRect>();
    }
    public void AddMent(string str, bool compareMentBool, int lieORinfoErrorValue)
    {
        scrollRect = GetComponent<ScrollRect>();
        var newUi=Instantiate(uiPrefab,scrollRect.content).GetComponent<RectTransform>();
        string txtAll =str;
        int lineCnt = 1;
        txtALlTmp.text = txtAll;
        string resultStr="";

        Debug.Log("해당 정보가 틀린가?: "+compareMentBool);

        //대화창 가로폭보다 긴 텍스트는 줄내림//
        if (txtALlTmp.preferredWidth >= 300)  
        {
            (resultStr, txtAll, lineCnt) =LineCut(str);
            
            if (compareMentBool == false)
            {
                newUi.GetComponent<ChangeTextTexture>().lastMent = txtAll;
            }
        }
        else//대화창 가로폭보다 짧은 텍스트는 그대로 출력//
        {
            resultStr = str;

            if (compareMentBool == false)
            {
                newUi.GetComponent<ChangeTextTexture>().lastMent = resultStr;
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

    }

    public void MakeMentList()
    {
        //전 사람 진술서 데이터 삭제
        if (uiObjects.Count != 0)
        {
            uiObjects.Clear();
            foreach (Transform child in content.transform)
            {
                Destroy(child.gameObject);
            }
        }

        attackerInfo = FindObjectOfType<AttackerInfo>();
        string[] fixtext = { "이름 : ", "죄목 : ", "발생장소 : ", "경위 : " };
        height = 5f;
        bool isNameTrue=false;
        /*
        for (int i = 0; i < 4; i++)
        {
            compareMentBool = attackerInfo.recordData.correctState[i].Equals(attackerInfo.recordData.currentState[i]) ? true : false;
            AddMent(fixtext[i] + attackerInfo.recordData.currentState[i], compareMentBool, attackerInfo.recordData.lieORinfoErrorValue);
        }
        */
        for (int i = 0; i < 5; i++)
        {
            if(i==0)
            {
                isNameTrue = attackerInfo.recordData.correctState[i].Equals(attackerInfo.recordData.currentState[i]) ? true : false;

            }
            else if (i==1)
            {
                compareMentBool = attackerInfo.recordData.correctState[i].Equals(attackerInfo.recordData.currentState[i]) ? true : false;
                if(isNameTrue==true && compareMentBool == true)
                {
                    AddMent(fixtext[i-1] + attackerInfo.recordData.currentState[i-1] +" "+  attackerInfo.recordData.currentState[i], true, attackerInfo.recordData.lieORinfoErrorValue);
                }
                else
                {
                    AddMent(fixtext[i - 1] + attackerInfo.recordData.currentState[i - 1] + " " + attackerInfo.recordData.currentState[i], false, attackerInfo.recordData.lieORinfoErrorValue);
                }
            }
            else
            {
                compareMentBool = attackerInfo.recordData.correctState[i].Equals(attackerInfo.recordData.currentState[i]) ? true : false;
                AddMent(fixtext[i-1] + attackerInfo.recordData.currentState[i], compareMentBool, attackerInfo.recordData.lieORinfoErrorValue);
            }
            
        }
       
    }

    public void MakeMentCangedList(List<string> currentList, List<bool> currentClick, List<bool> compareMentBoolList)
    {
        string[] fixtext = { "이름 : ", "죄목 : ", "발생장소 : ", "경위 : " };
        height = 5f;
        for (int i = 0; i < 5; i++)
        {
            if (i == 0)
            {
                AddChangedMent(currentList[i] + " " + currentList[i+1], compareMentBoolList[i], attackerInfo.recordData.lieORinfoErrorValue, currentClick[i]);
            }
            else if (i ==2 || i==3 || i == 4)
            {
                AddChangedMent(currentList[i], compareMentBoolList[i-1], attackerInfo.recordData.lieORinfoErrorValue, currentClick[i-1]);
            }
            //compareMentBool = attackerInfo.recordData.correctState[i].Equals(attackerInfo.recordData.currentState[i]) ? true : false;
            
        }
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
