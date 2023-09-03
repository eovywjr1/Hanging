using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextInformationOfMarker : MonoBehaviour
{
    [SerializeField] TMP_Text information;
    [SerializeField] WorkSystemData systemDataManager;

    private void Start()
    {
        information.text = "등급 표식 생성일 : " + systemDataManager.markerOccurDate.ToString("yyyy-MM-dd") + "\n";
        information.text += "승급일 : " + systemDataManager.promotionDate.ToString("yyyy-MM-dd") + "\n";
        information.text += "등급 표식 변경 사유 : " + systemDataManager.reasonForChange;
    }
}
