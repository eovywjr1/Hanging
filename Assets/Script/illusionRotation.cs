using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class illusionRotation : MonoBehaviour
{
    private Transform transform;

    public float velocity;

    private void Start()
    {
        transform = GetComponent<Transform>();
    }

    private void Update()
    {
        /*transform.rotation = Quaternion.Euler(0f, 0f, velocity * Time.deltaTime);*/
        transform.Rotate(0f, 0f, velocity * Time.deltaTime);
    }
}
