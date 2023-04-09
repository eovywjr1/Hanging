using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogEvent : MonoBehaviour
{
    DialogUpdateAndEvent dialogUpdateAndEvent;

    protected void Awake()
    {
        dialogUpdateAndEvent = FindObjectOfType<DialogUpdateAndEvent>();
    }

    public void SetSituationDialogEvent(int id, float time)
    {
        StartCoroutine(dialogUpdateAndEvent.SetSituationDialog(id, time));
    }

    public void SetCompulsoryDialogEvent(string name)
    {
        dialogUpdateAndEvent.SetValueByName(name);
    }

    public int GetRandomId(int minId, int maxId)
    {
        return Random.Range(minId, maxId + 1);
    }
}
