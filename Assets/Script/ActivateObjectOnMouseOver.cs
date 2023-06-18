using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateObjectOnMouseOver : MonoBehaviour
{
    [SerializeField] GameObject targetObject;

    private void Start()
    {
        targetObject.SetActive(false);
    }

    private void OnMouseEnter()
    {
        targetObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        targetObject.SetActive(false);
    }
}
