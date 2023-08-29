using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class padButton : MonoBehaviour
{
    [SerializeField] GameObject geneticCodeSearch;
    [SerializeField] CircleCollider2D geneBtnCollider;

    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D raycast = Physics2D.Raycast(mousePos, Vector2.zero);

        if(raycast.collider != null && Input.GetMouseButtonDown(0))
        {
            if(raycast.collider == geneBtnCollider)
            {
                if (geneticCodeSearch.activeSelf == true)
                    geneticCodeSearch.SetActive(false);
                else
                    geneticCodeSearch.SetActive(true);
            }
        }
    }
}
