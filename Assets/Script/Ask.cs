using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ask : MonoBehaviour
{
    [SerializeField] Button yes, no;
    [SerializeField] AttackerInfo attackerInfo;

    public void ActiveAsk()
    {
        yes.gameObject.SetActive(true);
        no.gameObject.SetActive(true);
    }

    public void StartAsk()
    {
        int accept = Random.Range(0, 2);
        if (accept == 1) attackerInfo.recordData.isHanging = 1;
        else attackerInfo.recordData.isHanging = 0;

        DisableAsk();
    }

    public void DisableAsk()
    {
        yes.gameObject.SetActive(false);
        no.gameObject.SetActive(false);
    }
}
