using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class illusionRotation : MonoBehaviour
{
    public float velocity;

    private void Update()
    {
        transform.Rotate(0f, 0f, velocity * Time.deltaTime);
    }
}
