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

    public IEnumerator ChangeTransparency(int mode)
    {
        List<Line> lineList = Line.lineList;
        float initAlpha = lineList[0].windowSR.color.a;
        float speed = 0.01f;
        float oper = (mode == 1) ? speed * 0.3f : -1 * speed;  //나타날 때 더 빠르게 보이는 경향 있음
        float d = initAlpha;

        while (d >= 0 && d <= 1)
        {
            foreach (Line line in lineList)
            {
                line.SetAlpha(d);
                yield return new WaitForSeconds(speed);
            }

            d += oper;
        }

        if(d <= 0.1)
        {
            foreach (Line line in lineList)
                line.SetAlpha(0);
        }
        else if(d >= 0.9)
        {
            foreach (Line line in lineList)
                line.SetAlpha(1);
        }
    }
}