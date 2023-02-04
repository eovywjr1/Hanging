using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CutRope : MonoBehaviour
{
    Vector3 initialMousePosition;
    public GameObject rope;
    private float cutDistance = 2f;
    HangingManager hangingManager;
    private bool isCut = false;

    private void Awake()
    {
        hangingManager = FindObjectOfType<HangingManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && rope != null)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            initialMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            isCut = true;
        }
        else if (isCut && Input.GetMouseButton(0))
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 ropePosition = rope.transform.position;

            if ((initialMousePosition.x >= ropePosition.x && initialMousePosition.x - currentMousePosition.x >= cutDistance && currentMousePosition.x < ropePosition.x) ||
                (initialMousePosition.x < ropePosition.x && currentMousePosition.x - initialMousePosition.x >= cutDistance && currentMousePosition.x > ropePosition.x))
            {
                hangingManager.UnTodesstrafe();
            }
        }
        else if (Input.GetMouseButtonUp(0)) isCut = false;
    }
}