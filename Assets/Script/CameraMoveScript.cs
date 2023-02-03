using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
    private bool isMove;
    private float speed = 3f;
    private float directionY = 1f;
    private int maxPositionY = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            directionY *= -1f;
            maxPositionY = (maxPositionY == -7) ? 0 : -7;
            isMove = true;
        }

        if (isMove)
        {
            transform.Translate(new Vector3(0, directionY, 0) * speed * Time.deltaTime);
            if (transform.position.y <= -7 || transform.position.y >= 0) isMove = false;
        }
    }
}
