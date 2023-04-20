using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ask : MonoBehaviour
{
    [SerializeField] AttackerInfo attackerInfo;
    bool isPossibleAsk, isActiveAsk;
    public static bool isFirst = true;

    public void ActiveAsk()
    {
        int accept = Random.Range(0, 2);
        if (accept == 0)
            rejectAsk();
        else
            acceptAsk();

        EventManager.instance.postNotification("dialogEvent", this, 29);
        EventManager.instance.postNotification("activeAsk", this, null);
    }

    public void acceptAsk()
    {
        if (attackerInfo.recordData.attackerData.ContainsKey("ask") && attackerInfo.recordData.attackerData["ask"].Equals("1"))
            attackerInfo.recordData.isHanging = 1;

        EventManager.instance.postNotification("dialogEvent", this, UnityEngine.Random.Range(30, 41));
    }

    public void rejectAsk()
    {
        EventManager.instance.postNotification("dialogEvent", this, UnityEngine.Random.Range(41, 53));
    }
}
