using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float y = (transform.position.y == 0) ? -10 : 0;
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
    }
}
