using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExlcamationmarkMouse : MonoBehaviour
{
    [SerializeField] GameObject dataPrefab;
    GameObject dataObject;
    SpriteRenderer spriteRenderer;
    RectTransform dataRectTransform;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        if (spriteRenderer.color.a >= 1)
        {
            if (dataObject == null)
            {
                dataObject = Instantiate(dataPrefab, transform.position, Quaternion.identity, transform);
                dataRectTransform = dataObject.GetComponent<RectTransform>();
            }
            Vector2 vector2 = new Vector2(transform.position.x + 1.2f, transform.position.y - dataRectTransform.sizeDelta.y / 2);
            dataObject.transform.position = vector2;
            dataObject.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        dataObject.SetActive(false);
    }
}
