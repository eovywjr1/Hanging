using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Line
{
    public static List<Line> lineList = new List<Line>();
    public GameObject lineObject { get; private set; }
    public GameObject windowObject { get; private set; }
    private LineRenderer lineLR;
    public Image windowImage;
    SpriteRenderer buttonSpriteRenderer;
    private TextMeshProUGUI windowtmpu;
    const float devide = 300f; //분모여서 속도와 반비례관계
    public float parentYSum;   //사형수, 교수대(부모들) y 추가
    public delegate void ShowData();
    ShowData showdata;

    [SerializeField] int expandStep = 50;   //유민 추가

    public Line(GameObject _lineObject, GameObject _windowObject)
    {
        lineObject = _lineObject;
        windowObject = _windowObject;
        lineLR = lineObject.GetComponent<LineRenderer>();
        windowImage = windowObject.GetComponent<Image>();
        windowtmpu = windowObject.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        buttonSpriteRenderer = windowObject.transform.GetChild(1).GetComponentInChildren<SpriteRenderer>();
    }

    public void SetAlpha(float alpha)
    {   
        //처음 라인 만들어질 때//
        if (alpha == -1)
        {
            windowImage.color = new Color(1, 1, 1, 0);
            windowtmpu.color = new Color(0, 0, 0, 0);
            buttonSpriteRenderer.color = new Color(1, 1, 1, 0);
        }
        else
        {
            lineLR.startColor = new Color(1, 1, 1, alpha * 3f);   //컬러 나중에 재조정 필요
            lineLR.endColor = new Color(1, 1, 1, alpha * 3f);
            windowImage.color = new Color(1, 1, 1, alpha);
            windowtmpu.color = new Color(0, 0, 0, alpha);
            buttonSpriteRenderer.color = new Color(1, 1, 1, alpha);
        }
    }

    public IEnumerator ChangeTransparency(int mode)
    {
        if (lineList.Count == 0)
            yield break;
            
        float initAlpha = lineList[0].windowImage.color.a;
        float speed = 0.02f;
        float oper = (mode == 1) ? speed * 0.5f : -1 * speed;  //나타날 때 더 빠르게 보이는 경향 있음
        float d = initAlpha;

        while (d >= 0 && d <= 1 && lineList.Count > 0)
        {
            foreach (Line line in lineList)
            {
                line.SetAlpha(d);
                yield return new WaitForSeconds(0.01f);
            }

            d += oper;
        }

        if (d <= 0.1)
        {
            foreach (Line line in lineList)
                line.SetAlpha(0);
        }
        else if (d >= 0.9)
        {
            foreach (Line line in lineList)
                line.SetAlpha(1);
        }
    }

    public void MoveTo(float x, float y)
    {
        lineLR.SetPosition(1, new Vector3(x, parentYSum + y, 0));
    }

    public void MoveTo(float x, float y, float parentX, float parentY)
    {
        lineLR.SetPosition(1, new Vector3(x - parentX, y - parentY, 0));
    }

    public IEnumerator ExpendLine(float _parentX, float _parentYSum)
    {
        /*int i = 1;
        float d = i / devide;
        while (d < 1)
        {
            parentYSum = _parentYSum;
            MoveTo((windowObject.transform.position.x - _parentX) * d, (_parentYSum + windowObject.transform.position.y) * d - _parentYSum);
            yield return null;
            d = ++i / devide;
        }

        SetAlpha(1);
        showdata();
        */

        parentYSum = _parentYSum;

        for(int i=0;i<=expandStep; i++)
        {
            float t = (float)i / expandStep;
            float x = (windowObject.transform.position.x - _parentX) * t;
            float y = (_parentYSum + windowObject.transform.position.y) * t - _parentYSum;

            MoveTo(x, y);

            yield return null;

        }

        SetAlpha(1);
        showdata();
    }

    public void GetShowData(ShowData _showData)
    {
        showdata = new ShowData(_showData);
    }
}