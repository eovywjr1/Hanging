using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float movePower = 3f;
    [SerializeField] float jumpPower = 3f;

    SpriteRenderer characterRenderer;
    Rigidbody2D rigid;
    Vector3 vector;

    bool isJumping = false;
    bool isRunning = false;

    private void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        characterRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if(Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;

            characterRenderer.flipX = true;
        }
        else if(Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;

            characterRenderer.flipX = false;
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void Jump()
    {
        if (!isJumping) return;

        rigid.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

        isJumping = false;
    }
}
