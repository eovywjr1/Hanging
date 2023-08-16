using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ask : MonoBehaviour, IListener
{
    AttackerInfo attackerInfo;
    public static bool isFirst = true;

    private void Start()
    {
        EventManager.instance.addListener("possibleAsk", this);
    }

    public void activeAsk()
    {
        attackerInfo = GetComponent<AttackerInfo>();
        if (attackerInfo == null)
            return;

        if (attackerInfo.checkAttackerPossibleAsk() == false)
            return;

        //ui 보여주기
        //버튼(예) 누르면 executeAsk 호출
    }

    public void executeAsk()
    {
        if (attackerInfo.recordData.attackerData["askReply"].Equals("1"))
            acceptAsk();
        else
            rejectAsk();

        EventManager.instance.postNotification("dialogEvent", this, 29);
        EventManager.instance.postNotification("executeAsk", this, null);
    }

    public void acceptAsk()
    {
        if (attackerInfo.checkAttackerAcceptAsk() && attackerInfo.checkAttackerReplyAsk())
            attackerInfo.recordData.isHanging = 0;

        EventManager.instance.postNotification("dialogEvent", this, UnityEngine.Random.Range(30, 41));
    }

    public void rejectAsk()
    {
        EventManager.instance.postNotification("dialogEvent", this, UnityEngine.Random.Range(41, 53));
    }

    public void OnEvent(string eventType, Component sender, object parameter = null)
    {
        switch (eventType)
        {
            case "possibleAsk":
                {

                    activeAsk();
                }
                break;
        }
    }
}
