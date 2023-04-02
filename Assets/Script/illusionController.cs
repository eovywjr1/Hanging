using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class illusionController : MonoBehaviour
{
    public static illusionController instance;
    public GameObject illusionMask;
    public SpriteMask spriteMask;

    //해당 bool값들 gameManager에서 관리 필요
    public bool state;
    public bool ill;
    public int step = 1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        state = false;
        illusionMask = GameObject.Find("illusionMask");
        spriteMask = illusionMask.GetComponent<SpriteMask>();
    }

    private void Update()
    {
        if (state)
        {
            gameObject.SetActive(true);
            state = true;
        }
        else
        {
            gameObject.SetActive(false);
            state = false;
        }

        if (ill)
        {
            illusion();
        }
    }

    public void illusion()
    {
         spriteMask.transform.localScale = new Vector3(
         /*transform.localScale.x*/ 140 - 1f * step * Time.deltaTime,
         /*transform.localScale.y*/ 65 - 1f * step * Time.deltaTime, 0
        );
/*
        spriteMask.transform.localScale = new Vector3(
            140 - (step*10), 65 - (step * 5), 0);*/
    }

}