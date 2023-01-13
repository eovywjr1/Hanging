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
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        initialMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        if (rope != null)
        {
            Vector3 ropePosition = rope.transform.position;
            if ((initialMousePosition.x >= ropePosition.x && initialMousePosition.x - currentMousePosition.x >= cutDistance && currentMousePosition.x < ropePosition.x) ||
                (initialMousePosition.x < ropePosition.x && currentMousePosition.x - initialMousePosition.x >= cutDistance && currentMousePosition.x > ropePosition.x))
            {
                hangingManager.UnTodesstrafe();
            }
        }
    }
}