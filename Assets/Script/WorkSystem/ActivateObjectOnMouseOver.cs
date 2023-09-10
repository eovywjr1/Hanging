using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObjectOnMouseOver : TextInformationOfMarker
{
    [SerializeField] GameObject targetObject;

    private void Start()
    {
        targetObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        targetObject.SetActive(true);
        SetText(this.gameObject.name);
    }

    private void OnMouseExit()
    {
        targetObject.SetActive(false);
    }
}
