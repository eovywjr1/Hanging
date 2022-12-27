using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [SerializeField] GameObject linePrefab;
    //public List<GameObject> lineList = new List<GameObject>();

    private void Start()
    {
        //hangingMove = FindObjectOfType<HangingMove>();
    }

    public void CreateLine(Transform parent)
    {
        GameObject line = Instantiate(linePrefab, parent.position, Quaternion.identity, parent);
        //lineList.Add(line);
    }
}

public class Line
{
    public static List<Line> lineList = new List<Line>();
    public GameObject lineObject;
    public GameObject windowObject;
    public float devide = 1000f; //분모여서 속도와 반비례관계
    public float parentYSum;   //사형수, 교수대(부모들) y 추가

    public Line(GameObject _lineObject, GameObject _windowObject)
    {
        lineObject = _lineObject;
        windowObject = _windowObject;
    }
}