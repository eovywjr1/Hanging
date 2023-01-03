using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
    public static List<Line> lineList = new List<Line>();
    public GameObject lineObject { get; private set; }
    public GameObject windowObject { get; private set; }
    private LineRenderer lineLR;
    public SpriteRenderer windowSR { get; private set; }
    public float devide = 1000f; //분모여서 속도와 반비례관계
    public float parentYSum;   //사형수, 교수대(부모들) y 추가

    public Line(GameObject _lineObject, GameObject _windowObject)
    {
        lineObject = _lineObject;
        windowObject = _windowObject;
        lineLR = lineObject.GetComponent<LineRenderer>();
        windowSR = windowObject.GetComponent<SpriteRenderer>();
    }

    public void SetAlpha(float alpha)
    {
        lineLR.startColor = new Color(1, 1, 1, alpha);   //컬러 나중에 재조정 필요
        lineLR.endColor = new Color(1, 1, 1, alpha);
        windowSR.color = new Color(0, 0, 0, alpha);
    }
}