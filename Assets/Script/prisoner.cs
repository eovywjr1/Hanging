using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prisoner : MonoBehaviour
{
    private BoxCollider2D p_boxCollider;
    private Animator p_animator;
    private Transform p_transform;

    public BoxCollider2D footCollider;

    private bool p_drag = false;

    void Start()
    {
        p_boxCollider = GetComponent<BoxCollider2D>();
        p_animator = GetComponent<Animator>();
        p_transform = GetComponent<Transform>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "floor")
        {
            Debug.Log("Floor!");
            AnimatorSetBool(true, false, false, false, false);
        }
    }

    private void OnMouseDrag()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = objPos;
        p_drag = true;

        p_animator.SetBool("idle", false);
        p_animator.SetBool("lowerRope", false);
        p_animator.SetBool("liftRope_ver1", true);
        p_animator.SetBool("liftRope_ver2", false);
        p_animator.SetBool("liftRope_ver3", false);

        /*        while(p_drag)
                {
                    randomLiftRope();
                }*/
    }

    private void OnMouseExit()
    {
        p_drag = false;
        /*AnimatorSetBool(false, true, false, false, false);*/
        p_animator.SetBool("idle", false);
        p_animator.SetBool("lowerRope", true);
        p_animator.SetBool("liftRope_ver1", false);
        p_animator.SetBool("liftRope_ver2", false);
        p_animator.SetBool("liftRope_ver3", false);
    }

    public void AnimatorSetBool(bool idle, bool lowerRope, bool lift1, bool lift2, bool lift3)
    {
        p_animator.SetBool("idle", idle);
        p_animator.SetBool("lowerRope", lowerRope);
        p_animator.SetBool("liftRope_ver1", lift1);
        p_animator.SetBool("liftRope_ver2", lift2);
        p_animator.SetBool("liftRope_ver3", lift3);
    }

    public void randomLiftRope()
    {
        int rand = Random.Range(0, 2);
        switch(rand)
        {
            case 0:
                AnimatorSetBool(false, false, true, false, false);
                break;
            case 1:
                AnimatorSetBool(false, false, false, true, false);
                break;
            default:
                break;
        }
    }
}
