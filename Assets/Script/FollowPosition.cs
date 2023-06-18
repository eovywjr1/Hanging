using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    Transform thisTransform;
    [SerializeField] Transform following;

    private void Awake()
    {
        thisTransform = this.GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        thisTransform.position = following.position;
    }
}
