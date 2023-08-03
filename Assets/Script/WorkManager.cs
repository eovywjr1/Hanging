using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkManager : MonoBehaviour
{
    [SerializeField] GameObject TortureSystem;

    public bool tortureCondition;

    private void Awake()
    {
        TortureSystem.SetActive(false);
    }

    private void Update()
    {
        if (tortureCondition)       //고문 조건 충족 시
        {
            TortureSystem.SetActive(true);
        }
        else if(!tortureCondition)  //고문 조건 미 충족 시 or 고문 2회 수행 시
        {
            TortureSystem.SetActive(false);
        }
    }
}
