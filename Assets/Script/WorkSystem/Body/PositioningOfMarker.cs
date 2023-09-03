using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositioningOfMarker : MonoBehaviour
{
    [SerializeField] GameObject parentOfPos;    //위치리스트의 부모
    public List<Transform> positionList;       //위치리스트

    public int gradePos;  //등급 위치
    public int scarPos;   //흉터 위치
    public int tattooPos; //문신 위치

    private void Awake()
    {
        positionList
            = new List<Transform>(parentOfPos.GetComponentsInChildren<Transform>());

        positionList.Remove(parentOfPos.transform);    //상위 오브젝트의 Transform은 제거

        SetRandomPos();
    }

    void SetRandomPos()
    {
        const int numOfMarkerTypes = 3;
        List<int> availableNumbers = new List<int>();
        List<int> selectedNumbes = new List<int>();

        for (int i = 0; i < positionList.Count; i++)
        {
            availableNumbers.Add(i);
        }

        for (int i = 0; i < numOfMarkerTypes; i++)
        {
            int randomIdx = Random.Range(0, availableNumbers.Count);
            int selectedNum = availableNumbers[randomIdx];

            selectedNumbes.Add(selectedNum);
            availableNumbers.RemoveAt(randomIdx);
        }

        gradePos = selectedNumbes[0];
        scarPos = selectedNumbes[1];
        tattooPos = selectedNumbes[2];
    }
}
