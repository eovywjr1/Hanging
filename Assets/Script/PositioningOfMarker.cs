using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositioningOfMarker : MonoBehaviour
{
    public GameObject prisoner;
    RecordData recordData;

    [SerializeField] GameObject marker;
    [SerializeField] List<Sprite> spriteList;

    [SerializeField] GameObject parentOfList;
    private List<Transform> positionList;
    private Transform position;

    SpriteRenderer spriteRenderer;

    [SerializeField] private int grade;
    [SerializeField] private int gradePos;
    [SerializeField] private int scarPos;
    [SerializeField] private int tattooPos;

    private void Awake()
    {
        prisoner = GameObject.Find("Prisoner(Clone)").gameObject;
        recordData = prisoner.GetComponent<AttackerInfo>().recordData;

        spriteRenderer = GetComponent<SpriteRenderer>();
        positionList 
            = new List<Transform>(parentOfList.GetComponentsInChildren<Transform>());
        
        positionList.Remove(parentOfList.transform);    //상위 오브젝트의 Transform은 제거
    }

    private void Start()
    {
        grade = int.Parse(recordData.attackerData["positionGrade"]);
        //Debug.Log("사형수의 등급은 " + grade + "!!");
        int random = Random.Range(0, positionList.Count);
        int randomIdx = Random.Range(0, spriteList.Count);
        string objName = this.gameObject.name;
        
        switch(objName)
        {
            case "시민등급표식":
                spriteRenderer.sprite = spriteList[grade - 1];
                position = positionList[gradePos];
                break;
            case "문신":
                spriteRenderer.sprite = spriteList[randomIdx];
                position = positionList[tattooPos];
                break;
            case "흉터":
                spriteRenderer.sprite = spriteList[randomIdx];
                position = positionList[scarPos];
                break;
            default:
                break;
        }

        marker.transform.position = position.position;
    }

    void SetRandomPos()
    {
        
    }
}
