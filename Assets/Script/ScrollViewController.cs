using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour
{
    private ScrollRect scrollRect;
    public float space = 0f;

    public GameObject uiPrefab;
    public List<RectTransform> uiObjects= new List<RectTransform>();
    public AttackerInfo attackerInfo;
    public TMP_Text txtALlTmp;
    public TMP_Text forWidth;
    private bool compareMent;
    private bool flag;
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
            string[] fixtext = { "이름 : ", "범죄 : ", "범죄장소 : ", "범죄경위 : " };
            for (int i = 0; i < 4; i++)
            {
                compareMent = attackerInfo.recordData.correctState[i].Equals(attackerInfo.recordData.currentState[i]) ? true : false;
                AddMent(fixtext[i] + attackerInfo.recordData.currentState[i], compareMent, attackerInfo.recordData.lieORinfoErrorValue);
            }
            flag = true;
            //MakeMentList();
            
        }
       
    }
    public void AddMent(string str, bool compareMent, int lieORinfoErrorValue)
    {
        var newUi=Instantiate(uiPrefab,scrollRect.content).GetComponent<RectTransform>();

        //str = "안녕하세요안녕하세요안녕하세요안녕하세요안녕하세요";

        string txtAll =str;
        int lineCnt = 1;
        if (txtALlTmp == null) Debug.Log("업승ㅁ"); 
        //txtALlTmp=new tmp
        txtALlTmp.text = txtAll;

        int a = 0;
        string resultStr="";
        string addStr="";

        //글자가 창을 넘어가는 길이면 개행문자 추가함//
        if (txtALlTmp.preferredWidth >= 300)  
        {
            while (txtALlTmp.preferredWidth > 300)
            {
                //초기화
                addStr = "";
                forWidth.text = "";

                for(int i = 0; forWidth.preferredWidth<= 300; i++) 
                {
                    addStr = addStr + txtAll[0]; 
                    txtAll = txtAll.Remove(0, 1);
                    txtALlTmp.text = txtAll;

                    forWidth.text = addStr; // addStr의 길이를 알아내기 위함
                    
                }
                lineCnt++;
                resultStr =resultStr+ addStr+System.Environment.NewLine; //줄바꿈

                
            }

            resultStr = resultStr + txtAll;

            //정보다를때만 마지막 문장 저장
            if (compareMent == false)
            {
                newUi.GetComponent<ChangeTextTexture>().lastMent = txtAll;
            }
        }
        else {
            //txtALlTmp.text = resultStr; //위증/정보오류 글씨 길이 판단을 위함
            //정보다를때만 마지막 문장 저장
            if (compareMent == false)
            {
                newUi.GetComponent<ChangeTextTexture>().lastMent = resultStr;
            }

            resultStr = str;
        }

        /*
        //위증/정보오류 글씨 추가
        if (lieORinfoErrorValue == 1) //위증
        {
            txtALlTmp.text = addStr + "불일치 -> 위증";
            if (txtALlTmp.preferredWidth >= 300)
            {
                //위증 글씨만 아래줄로
                resultStr = resultStr + System.Environment.NewLine + "불일치 -> 위증";
            }
            else
            {
                //위증 글씨 그냥 추가
                resultStr = resultStr + "불일치 -> 위증";
            }
        }
        */

        //Debug.Log("resultStr : "+resultStr);

        newUi.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = resultStr;
        newUi.GetComponent<ChangeTextTexture>().mentTureORFalse = compareMent;
        newUi.GetComponent<ChangeTextTexture>().lieORinfoErrorValue = lieORinfoErrorValue;
        
        
        uiObjects.Add(newUi);

        float y = 0f;
        for(int i = 0; i < uiObjects.Count; i++) {
            uiObjects[i].anchoredPosition = new Vector2(0f, -y);
            y +=  9+ space; //9*lineCnt
            Debug.Log("y : "+uiObjects[i].sizeDelta.y);
        }

        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, y);

        Debug.Log("맨트 추가");
    }

    /*
    void ShowMent()
    {
        Debug.Log("어쩔fkrh라고" );
        for (int i = 0; i < 4; i++)
        {
            //Debug.Log("어쩔"+attackerInfo.recordData.isHanging);
            compareMent = attackerInfo.recordData.correctState[i].Equals(attackerInfo.recordData.currentState[i]) ? true : false;
            AddMent(attackerInfo.recordData.currentState[i], compareMent);
        }
    }
    */
    void MakeMentList()
    {
        string[] fixtext = { "이름 : ", "범죄 : ", "범죄장소 : ", "범죄경위 : " };
        for (int i = 0; i < 4; i++)
        {
            compareMent = attackerInfo.recordData.correctState[i].Equals(attackerInfo.recordData.currentState[i]) ? true : false;
            AddMent(fixtext[i] + attackerInfo.recordData.currentState[i], compareMent, attackerInfo.recordData.lieORinfoErrorValue);
        }
        flag = true;
    }
}
