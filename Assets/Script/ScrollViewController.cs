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
            string[] fixtext = { "�̸� : ", "���� : ", "������� : ", "���˰��� : " };
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

        //str = "�ȳ��ϼ���ȳ��ϼ���ȳ��ϼ���ȳ��ϼ���ȳ��ϼ���";

        string txtAll =str;
        int lineCnt = 1;
        if (txtALlTmp == null) Debug.Log("���¤�"); 
        //txtALlTmp=new tmp
        txtALlTmp.text = txtAll;

        int a = 0;
        string resultStr="";
        string addStr="";

        //���ڰ� â�� �Ѿ�� ���̸� ���๮�� �߰���//
        if (txtALlTmp.preferredWidth >= 300)  
        {
            while (txtALlTmp.preferredWidth > 300)
            {
                //�ʱ�ȭ
                addStr = "";
                forWidth.text = "";

                for(int i = 0; forWidth.preferredWidth<= 300; i++) 
                {
                    addStr = addStr + txtAll[0]; 
                    txtAll = txtAll.Remove(0, 1);
                    txtALlTmp.text = txtAll;

                    forWidth.text = addStr; // addStr�� ���̸� �˾Ƴ��� ����
                    
                }
                lineCnt++;
                resultStr =resultStr+ addStr+System.Environment.NewLine; //�ٹٲ�

                
            }

            resultStr = resultStr + txtAll;

            //�����ٸ����� ������ ���� ����
            if (compareMent == false)
            {
                newUi.GetComponent<ChangeTextTexture>().lastMent = txtAll;
            }
        }
        else {
            //txtALlTmp.text = resultStr; //����/�������� �۾� ���� �Ǵ��� ����
            //�����ٸ����� ������ ���� ����
            if (compareMent == false)
            {
                newUi.GetComponent<ChangeTextTexture>().lastMent = resultStr;
            }

            resultStr = str;
        }

        /*
        //����/�������� �۾� �߰�
        if (lieORinfoErrorValue == 1) //����
        {
            txtALlTmp.text = addStr + "����ġ -> ����";
            if (txtALlTmp.preferredWidth >= 300)
            {
                //���� �۾��� �Ʒ��ٷ�
                resultStr = resultStr + System.Environment.NewLine + "����ġ -> ����";
            }
            else
            {
                //���� �۾� �׳� �߰�
                resultStr = resultStr + "����ġ -> ����";
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

        Debug.Log("��Ʈ �߰�");
    }

    /*
    void ShowMent()
    {
        Debug.Log("��¿fkrh���" );
        for (int i = 0; i < 4; i++)
        {
            //Debug.Log("��¿"+attackerInfo.recordData.isHanging);
            compareMent = attackerInfo.recordData.correctState[i].Equals(attackerInfo.recordData.currentState[i]) ? true : false;
            AddMent(attackerInfo.recordData.currentState[i], compareMent);
        }
    }
    */
    void MakeMentList()
    {
        string[] fixtext = { "�̸� : ", "���� : ", "������� : ", "���˰��� : " };
        for (int i = 0; i < 4; i++)
        {
            compareMent = attackerInfo.recordData.correctState[i].Equals(attackerInfo.recordData.currentState[i]) ? true : false;
            AddMent(fixtext[i] + attackerInfo.recordData.currentState[i], compareMent, attackerInfo.recordData.lieORinfoErrorValue);
        }
        flag = true;
    }
}
