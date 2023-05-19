using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MakeStamement : MonoBehaviour
{
    public GameObject uiPrefab;
    public AttackerInfo attackerInfo;
    public TMP_Text txtALlTmp;
    public TMP_Text forWidth;
    private bool compareMent;
    private bool flag;
    private void Start()
    {
        attackerInfo= FindObjectOfType<AttackerInfo>();
        flag = false;
        //MakeMentList();
    }
    private void Update()
    {
        if (flag == false)
        {
            
            flag = true;
            MakeMentList();

        }
    }
    public void AddMent(string str, bool compareMent, int lieORinfoErrorValue)
    {
        var newUi = Instantiate(uiPrefab, this.transform.position, Quaternion.identity);
        

        //str = "안녕하세요안녕하세요안녕하세요안녕하세요안녕하세요";

        string txtAll = str;
        int lineCnt = 1;
        if (txtALlTmp == null) Debug.Log("업승ㅁ");
        //txtALlTmp=new tmp
        txtALlTmp.text = txtAll;

        string resultStr = "";
        string addStr = "";

        //글자가 창을 넘어가는 길이면 개행문자 추가함//
        if (txtALlTmp.preferredWidth >= 300)
        {
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

                    forWidth.text = addStr; // addStr의 길이를 알아내기 위함

                }
                lineCnt++;
                resultStr = resultStr + addStr + System.Environment.NewLine; //줄바꿈


            }

            resultStr = resultStr + txtAll;

            //정보다를때만 마지막 문장 저장
            if (compareMent == false)
            {
                newUi.GetComponent<ChangeTextTexture>().lastMent = txtAll;
            }
        }
        else
        {
            //txtALlTmp.text = resultStr; //위증/정보오류 글씨 길이 판단을 위함
            //정보다를때만 마지막 문장 저장
            if (compareMent == false)
            {
                newUi.GetComponent<ChangeTextTexture>().lastMent = resultStr;
            }

            resultStr = str;
        }


        newUi.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = resultStr;
        newUi.GetComponent<ChangeTextTexture>().mentTureORFalse = compareMent;
        newUi.GetComponent<ChangeTextTexture>().lieORinfoErrorValue = lieORinfoErrorValue;


        newUi.transform.parent = this.transform;

        Debug.Log("맨트 추가");
    }
    void MakeMentList()
    {
        string[] fixtext = { "이름 : ", "범죄 : ", "범죄장소 : ", "범죄경위 : " };
        for (int i = 0; i < 4; i++)
        {
            compareMent = attackerInfo.recordData.correctState[i].Equals(attackerInfo.recordData.currentState[i]) ? true : false;
            AddMent(fixtext[i] + attackerInfo.recordData.currentState[i], compareMent, attackerInfo.recordData.lieORinfoErrorValue);
        }
    }
}
