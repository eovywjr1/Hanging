using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [SerializeField] GameObject linePrefab;
    List<GameObject> lineList = new List<GameObject>();

    public void CreateLine(Transform parent)
    {
        GameObject line = Instantiate(linePrefab, parent.position, Quaternion.identity, parent);
        lineList.Add(line);
    }
}
