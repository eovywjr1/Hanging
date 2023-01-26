using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;

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
        uiBox.SetActive(false);
        

        line = new Line(this.gameObject, uiBox);
        Line.lineList.Add(line);

        parentTransform = transform.parent.transform;
        uiBox.transform.position = new Vector3(parentTransform.position.x + 4, parentTransform.position.y, 0);
    }

    private void Start()
    {
        parentYSum = (transform.parent.transform != null) ? (parentTransform.position.y * -1) : 0;
        StartCoroutine(line.ExpendLine(parentTransform.position.x, parentYSum));
    }
}