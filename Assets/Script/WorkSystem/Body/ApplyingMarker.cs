using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyingMarker : MonoBehaviour
{
    //marker == (등급표식 or 흉터 or 문신) 으로 주석 달았음

    public GameObject prisoner;
    RecordData recordData;
    PositioningOfMarker positioningOfMarker;

    [SerializeField] GameObject marker; //선택된 marker
    [SerializeField] List<Sprite> markerList;   //marker (후보?) 리스트

    private Transform position;                 //marker의 최종 위치

    SpriteRenderer spriteRenderer;

    [SerializeField] private int grade;     //등급

    private void Awake()
    {
        prisoner = GameObject.Find("Prisoner(Clone)").gameObject;
        recordData = prisoner.GetComponent<AttackerInfo>().recordData;
        positioningOfMarker = FindObjectOfType<PositioningOfMarker>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        grade = int.Parse(recordData.attackerData["positionGrade"]);
        //Debug.Log("사형수의 등급은 " + grade + "!!");
        int randomIdx = Random.Range(0, markerList.Count);
        string objName = this.gameObject.name;

        switch(objName)
        {
            case "시민등급표식":
                spriteRenderer.sprite = markerList[grade - 1];
                position = positioningOfMarker.positionList[positioningOfMarker.gradePos];
                Debug.Log("시민 등급 표식의 위치는 " + positioningOfMarker.gradePos);
                break;
            case "문신":
                spriteRenderer.sprite = markerList[randomIdx];
                position = positioningOfMarker.positionList[positioningOfMarker.tattooPos];
                Debug.Log("문신 표식의 위치는 " + positioningOfMarker.tattooPos);
                break;
            case "흉터":
                spriteRenderer.sprite = markerList[randomIdx];
                position = positioningOfMarker.positionList[positioningOfMarker.scarPos];
                Debug.Log("흉터 표식의 위치는 " + positioningOfMarker.scarPos);
                break;
            default:
                break;
        }

        marker.transform.position = position.position;
    }
}
