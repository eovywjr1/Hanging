using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NotClickOther : MonoBehaviour
{
    [SerializeField]
    private AttackerMouseMove attackerMouseMove;
    [SerializeField]
    private Rope rope;

    private void Awake()
    {
        attackerMouseMove = GameObject.Find("Prisoner").GetComponent<AttackerMouseMove>();
        rope = GameObject.Find("rope").GetComponent<Rope>();
    }
    void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            attackerMouseMove.SetPossibleTodesstrafe(false);
            Debug.Log("사형수 클릭 막기");
            rope.SetCutPossible(false);
            Debug.Log("로프 컷 막기");
        }
        else
        {
            attackerMouseMove.SetPossibleTodesstrafe(true);
            Debug.Log("사형수 클릭 풀기");
            rope.SetCutPossible(true);
            Debug.Log("로프 컷 풀기");
        }
    }
}
