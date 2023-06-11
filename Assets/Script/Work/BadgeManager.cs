using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadgeManager : MonoBehaviour
{
    [SerializeField] private GameObject badgePrefab;

    public void spawnBadge(int size)
    {
        if(badgePrefab == null)
        {
            Debug.Assert(false, "조민수 : BadgeWrap에 badgePrefab이 없어서 확인부탁드립니다.");
            return;
        }

        for (int index = 0; index < size; ++index)
        {
            Vector3 vector3 = transform.position;
            vector3.x = index - 1;
            Instantiate(badgePrefab, vector3, Quaternion.identity, transform);
        }
    }
}
