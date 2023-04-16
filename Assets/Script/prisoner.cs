using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prisoner : MonoBehaviour
{

    public CapsuleCollider2D PrisonerCollider;
    private Rigidbody2D PrisonerRigidbody;
    private Animator PrisonerAnimator;

    public AnimationClip []liftAnimations;

    public bool isLift = false;
    bool isRunningAnim = false;

    void Start()
    {
        PrisonerCollider = GetComponentInChildren<CapsuleCollider2D>();
        PrisonerRigidbody = GetComponent<Rigidbody2D>();
        PrisonerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isLift)
        {
            onHanging();
        }
        else
        {
            isRunningAnim = false; 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "floor")
        {
            PrisonerAnimator.SetBool("cutRope", true);
            PrisonerAnimator.SetBool("idle", false);
            PrisonerRigidbody.velocity = Vector3.zero;
            Debug.Log("cutRope!");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="floor")
        {
            PrisonerAnimator.SetBool("cutRope", false);
            PrisonerAnimator.SetBool("idle", true);
            PrisonerRigidbody.gravityScale = 1f;
        }
    }

    void onHanging()
    {
        if(!isRunningAnim)
        {
            int randomIndex = Random.Range(0, 3);
            PrisonerAnimator.Play(liftAnimations[randomIndex].name);
            isRunningAnim = true;
        }
    }
}
