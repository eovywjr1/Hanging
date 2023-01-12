using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutRope : MonoBehaviour
{
    Vector3 initialMousePosition;
    public GameObject rope;
    private float cutDistance = 2f;
    HangingManager hangingManager;

    private void Awake()
    {
        hangingManager = FindObjectOfType<HangingManager>();
    }

    private void OnMouseDown()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 initialMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Debug.Log(initialMousePosition);
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (rope != null)
        {
            if((initialMousePosition.x >= rope.transform.position.x && initialMousePosition.x - currentMousePosition.x >= cutDistance && currentMousePosition.x < rope.transform.position.x) ||
                (initialMousePosition.x < rope.transform.position.x && currentMousePosition.x - initialMousePosition.x >= cutDistance && currentMousePosition.x > rope.transform.position.x))
            {
                hangingManager.UnTodesstrafe();
            }
        }
    }
}
