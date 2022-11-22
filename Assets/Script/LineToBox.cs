using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToBox : MonoBehaviour
{
    [SerializeField] private GameObject uiBox;
    private Transform uiBoxTransform, parentTransform, grandparentTransform;
    private LineRenderer lineRenderer;
    private float devide = 1000f; //분모여서 속도와 반비례관계

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        uiBoxTransform = uiBox.transform;
        parentTransform = transform.parent.transform;
        grandparentTransform = parentTransform.parent.transform;
        StartCoroutine(ExpendLine());
    }

    IEnumerator ExpendLine()
    {
        int i = 1;
        float d = i / devide;
        float parentYSum = (parentTransform.position.y + grandparentTransform.position.y) * -1;
        while (d < 1)
        {
            lineRenderer.SetPosition(1, new Vector3(uiBoxTransform.position.x * d, (parentYSum + uiBoxTransform.position.y) * d, 0));
            yield return null;
            d = ++i / devide;
        }
    }
}
