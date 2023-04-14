using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class LineToBox : MonoBehaviour
{
    [SerializeField] private GameObject[] uiBoxPrefab;
    private GameObject uiBox;
    private Transform parentTransform;
    private Line line;
    private float parentYSum;

    private void Awake()
    {
        //기록서 생성 및 초기화//
        uiBox = Instantiate(uiBoxPrefab[Line.lineList.Count]);
        line = new Line(this.gameObject, uiBox);
        line.SetAlpha(-1);
        Line.lineList.Add(line);
        parentTransform = transform.parent.transform;
        uiBox.transform.position = new Vector3(parentTransform.position.x + 4, parentTransform.position.y, 0);
    }

    private void Start()
    {
        StartCoroutine(WaitCompleteRecordPosition());
    }

    IEnumerator WaitCompleteRecordPosition()
    {
        //위치 결정 기다리기
        RecordStartPosition recordStartPosition = uiBox.GetComponent<RecordStartPosition>();
        while (recordStartPosition.isPrepare == false) yield return null;

        parentYSum = (transform.parent.transform != null) ? (parentTransform.position.y * -1) : 0;
        StartCoroutine(line.ExpendLine(parentTransform.position.x, parentYSum));
    }
}