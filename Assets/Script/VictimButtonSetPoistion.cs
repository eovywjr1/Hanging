using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictimButtonSetPoistion : MonoBehaviour
{
    RecordData recordData;

    private void Awake()
    {
        recordData = FindObjectOfType<AttackerInfo>().recordData;

        int length = recordData.victimData["familyName"].Length + recordData.victimData["name"].Length - 3;
        Debug.Log("length" + length);
        transform.position = new Vector2(transform.position.x + 0.11f * length, transform.position.y);
    }
}
