using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextInformationOfMarker : MonoBehaviour
{
    [SerializeField] TMP_Text information;
    [SerializeField] WorkSystemData systemDataManager;

    public string inforText;

    //Marker(승급, 흉터, 문신) 정보에 대한 텍스트 정보
    //정확하게 기획된걸 받은게 등급표식에 대한것밖에 없어서 이부분은 추후 수정 필요

    public void SetText(string objName)
    {
        //Debug.Log("오브젝트 이름은 " + objName);

        if (objName == "시민등급표식")
            SetGradeText();
        else if (objName == "문신")
            SetTattooText();
        else if (objName == "흉터")
            SetScarText();
        else
            Debug.Log("오브젝트 이름 NULL!!");
    }

    private void SetGradeText()
    {
        information.text = "등급 표식 생성일 : " + systemDataManager.markerOccurDate.ToString("yyyy-MM-dd") + "\n";
        information.text += "승급일 : " + systemDataManager.promotionDate.ToString("yyyy-MM-dd") + "\n";
        information.text += "등급 표식 변경 사유 : " + systemDataManager.reasonForChange;
    }

    private void SetScarText()
    {
        information.text = "This is ScarInfo" + "\n";
    }

    private void SetTattooText()
    {
        information.text = "This is TattooInfo" + "\n";
    }
}
