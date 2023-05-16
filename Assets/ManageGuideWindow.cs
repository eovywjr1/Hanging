using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
                    attackerMouseMove.SetPossibleTodesstrafe(false);
                    //Debug.Log("사형수 클릭 막기");
                    rope.SetCutPossible(false);
                    //Debug.Log("로프 컷 막기");
                }
        }
        else
        {
            attackerMouseMove.SetPossibleTodesstrafe(true);
            //Debug.Log("사형수 클릭 풀기");
            rope.SetCutPossible(true);
            //Debug.Log("로프 컷 풀기");
        }
    }
}
