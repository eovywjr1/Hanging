using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using hanging;

public class ManageGuideWindow : MonoBehaviour
{
    public UiManager uiManager;

    [SerializeField]
    private AttackerMouseMove attackerMouseMove;
    [SerializeField]
    private Rope rope;

    GameObject prisoner;

    public RectTransform targetRectTransform;

    void Awake()
    {
        prisoner = GameObject.Find("Prisoner(Clone)");
        attackerMouseMove = prisoner.GetComponent<AttackerMouseMove>();
        rope = prisoner.GetComponentInChildren<Rope>();
    }

    void FixedUpdate()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(targetRectTransform, Input.mousePosition))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    rope.SetCutPossible(false);
                    //Debug.Log("·ÎÇÁ ÄÆ ¸·±â");
                }
        }
        else
        {
            rope.SetCutPossible(true);
            //Debug.Log("·ÎÇÁ ÄÆ Ç®±â");
        }
    }
}
