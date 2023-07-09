using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextInformationOfMarker : MonoBehaviour
{
    [SerializeField] TMP_Text information;
    [SerializeField] BodySearchData bodySearchData;

    private void Start()
    {
        information.text = "등급 표식 생성일 : " + bodySearchData.randomDate1.ToString("yyyy-MM-dd") + "\n";
        information.text += "승급일 : " + bodySearchData.randomDate2.ToString("yyyy-MM-dd") + "\n";
        information.text += "등급 표식 변경 사유 : " + bodySearchData.reasonForChange;
    }
}
