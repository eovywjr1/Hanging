using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ropeCutter : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetMouseButton(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if(hit.transform.CompareTag("rope"))
            {
                Debug.Log("Rope Cut!!");
                Destroy(hit.transform.gameObject);
            }
        }
    }
}
