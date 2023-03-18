using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExlcamationmarkMouse : MonoBehaviour
{
    [SerializeField] GameObject dataPrefab;
    GameObject dataObject;

    private void OnMouseEnter()
    {
        Vector2 vector2 = new Vector2(transform.position.x + 1.2f, transform.position.y - 1f);
        if (dataObject == null) dataObject = Instantiate(dataPrefab, vector2, Quaternion.identity, transform);
        dataObject.transform.position = vector2;
        dataObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        dataObject.SetActive(false);
    }
}
