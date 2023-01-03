using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class LineToBox : MonoBehaviour
{
    [SerializeField] private GameObject uiBoxPrefab;
    private GameObject uiBox;
    private Transform uiBoxTransform, parentTransform;
    private LineRenderer lineRenderer;
    private Line line;

    private void Awake()
    {
        uiBox = Instantiate(uiBoxPrefab);
        uiBox.GetComponent<WindowMouseMove>().SetpreLine(this);
        uiBox.SetActive(false);

        line = new Line(this.gameObject, uiBox);
        Line.lineList.Add(line);

        lineRenderer = GetComponent<LineRenderer>();
        uiBoxTransform = uiBox.transform;

        parentTransform = transform.parent.transform;
    }

    private void Start()
    {
        StartCoroutine(ExpendLine());
    }

    IEnumerator ExpendLine()
    {
        int i = 1;
        float d = i / line.devide;
        while (d < 1)
        {
            MoveToBox(uiBoxTransform.position.x * d, (line.parentYSum + uiBoxTransform.position.y) * d - line.parentYSum);
            yield return null;
            d = ++i / line.devide;
        }

        uiBox.SetActive(true);
    }

    public void MoveToBox(float x, float y)
    {
        line.parentYSum = (transform.parent.transform != null) ? (parentTransform.position.y * -1) : 0;
        lineRenderer.SetPosition(1, new Vector3(x, line.parentYSum + y, 0));
    }
}