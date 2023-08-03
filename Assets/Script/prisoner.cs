using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prisoner : MonoBehaviour
{
    public BoxCollider2D prisonerCollider;
    public BoxCollider2D parentsCollider;
    public BoxCollider2D footCollider;
    private Rigidbody2D prisonerRigidbody;
    private Animator animator;

    public AnimationClip[] liftAnimations;

    public bool isLift = false;
    public bool isCutRope = false;
    private bool isBeingDragged = false;
    private bool isCollidingWithGround = false;
    public bool isPlayingTortureAnim = false;

    void Start()
    {
        prisonerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (isCollidingWithGround)
        {
            //prisonerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
        else
        {
            //prisonerRigidbody.constraints = RigidbodyConstraints2D.None;
        }
    }

    private void Update()
    {
        if (isLift && !isBeingDragged)
        {
            isBeingDragged = true;
            int randomIndex = Random.Range(0, 3);
            animator.Play(liftAnimations[randomIndex].name);
            animator.SetBool("idle", false);
        }
        else if (!isLift && isBeingDragged)
        {
            isBeingDragged = false;
            animator.SetBool("idle", true);
        }

        if (isCutRope)
        {
            parentsCollider.enabled = false;
            footCollider.enabled = false;
            prisonerCollider.enabled = true;

            prisonerRigidbody.constraints = RigidbodyConstraints2D.None;

            animator.SetBool("cutRope", true);
        }
    }

    //애니메이션 수정 필요
    public void PlayTortureAnimation(int level)
    {
        if(isPlayingTortureAnim)
        {
            animator.SetBool("shock" + level.ToString(), true);
            animator.SetBool("idle", false);
            isPlayingTortureAnim = false;
        }
    }

    public void StopTortureAnimation(int level)
    {
        if (!isPlayingTortureAnim)
        {
            animator.SetBool("shock" + level.ToString(), false);
            animator.SetBool("idle", true);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            isCollidingWithGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            isCollidingWithGround = false;
        }
    }
}
