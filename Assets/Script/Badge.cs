using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Badge : MonoBehaviour
{
    public static Badge instance;

    public bool stopDrag;

    private void Awake()
    {
        instance = this;
    }

    /*    private void Update()
        {
            //BossHand에 Badge 제출 시 드래그 X
            if (BossHand.instance.isSubmit)
                stopDrag = true;
        }*/

    private void OnMouseDown()
    {
        //BossHand에 Badge 제출 시 드래그 X
        if (BossHand.instance.isSubmit)
            stopDrag = true;
    }

    private void OnMouseDrag()
    {
        if (!stopDrag)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = pos;
        }
    }
}
