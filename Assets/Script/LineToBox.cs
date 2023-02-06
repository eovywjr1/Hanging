using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class LineToBox : MonoBehaviour
{
    [SerializeField] private GameObject uiBoxPrefab;
    private GameObject uiBox;
    private Transform parentTransform;
    private Line line;
    private float parentYSum;

    private void Awake()
    {
        uiBox = Instantiate(uiBoxPrefab);
        uiBox.GetComponent<Image>().enabled = false;
        line = new Line(this.gameObject, uiBox);
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
        //��ġ ���� ��ٸ���
        RecordStartPosition recordStartPosition = uiBox.GetComponent<RecordStartPosition>();
        while (!recordStartPosition.isPrepare) yield return null;

        parentYSum = (transform.parent.transform != null) ? (parentTransform.position.y * -1) : 0;
        StartCoroutine(line.ExpendLine(parentTransform.position.x, parentYSum));
    }
}