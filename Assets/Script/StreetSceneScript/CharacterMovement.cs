using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float movePower = 3f;

    Transform characterTransform;
    Animator animator;

    private void Start()
    {
        characterTransform = gameObject.GetComponent<Transform>();
        animator = gameObject.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(Input.GetAxisRaw("Horizontal") < 0 || Input.GetAxisRaw("Horizontal") > 0)
        {
            animator.SetBool("isWalk", true);
            animator.SetBool("isIdle", false);
        }
        else
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isIdle", true);
        }

        Move();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if(Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;

            characterTransform.localScale = new Vector3(-0.5f, 0.5f, 1);
        }
        else if(Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;

            characterTransform.localScale = new Vector3(0.5f, 0.5f, 1);
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }
}
