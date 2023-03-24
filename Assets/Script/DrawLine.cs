using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer line;
    private Vector3 mousePos;
    public Material material;
    private int currLines;

    public int mousePosZ = -5;

    private void Start()
    {
        currLines = 0;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            if (line == null)
            {
                createLine();
            }
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = mousePosZ;
            line.SetPosition(0, mousePos);
            line.SetPosition(1, mousePos);
        }
        else if(Input.GetMouseButtonUp(1) && line)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = mousePosZ;
            line.SetPosition(1, mousePos);
            line = null;
            currLines++;
        }
        else if(Input.GetMouseButton(1) && line)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = mousePosZ;
            line.SetPosition(1, mousePos);
        }
    }

    void createLine()
    {
        line = new GameObject("Line" + currLines).AddComponent<LineRenderer>();
        line.material = material;
        line.positionCount = 2;
        line.startWidth = 0.15f;
        line.endWidth = 0.15f;
        line.useWorldSpace = true;
        line.numCapVertices = 50;
    }
}
