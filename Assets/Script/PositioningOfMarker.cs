using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositioningOfMarker : MonoBehaviour
{
    [SerializeField] GameObject marker;

    [SerializeField] GameObject parentOfList;
    private List<Transform> positionList;
    private Transform position;

    private void Awake()
    {
        positionList 
            = new List<Transform>(parentOfList.GetComponentsInChildren<Transform>());
        
        positionList.Remove(parentOfList.transform);    //상위 오브젝트의 Transform은 제거
    }

    private void Start()
    {
        int random = Random.Range(0, positionList.Count);

        position = positionList[random];

        marker.transform.position = position.position;
    }
}
