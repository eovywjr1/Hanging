using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [SerializeField] GameObject linePrefab;

    public void CreateLine(Transform parent)
    {
        GameObject line = Instantiate(linePrefab, parent.position, Quaternion.identity, parent);
    }
}