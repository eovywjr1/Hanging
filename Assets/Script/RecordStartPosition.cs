using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordStartPosition : MonoBehaviour
{
    private bool isAttackOverlap = false;
    public bool isPrepare = false;

    void Start()
    {
        StartCoroutine(RandomPosition());
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer.Equals("Attacker")) isAttackOverlap = true;
    }

    IEnumerator RandomPosition()
    {
        bool isRecordOverlap = false;
        while (true)
        {
            float x = Random.Range(-0.8f, 0.8f), y = Random.Range(-0.425f, 0.425f);
            Vector3 viewPos = Camera.main.WorldToViewportPoint(new Vector3(x, y, 0));
            Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
            transform.position = worldPos;
            
            if(Line.lineList.Count > 1)
            {
                GameObject firstRecord = Line.lineList[0].windowObject;

                if (Mathf.Abs(firstRecord.transform.position.x - transform.position.x) <= transform.localScale.x / 2 &&
                    Mathf.Abs(firstRecord.transform.position.y - transform.position.y) <= transform.localScale.y / 2)
                    isRecordOverlap = true;
            }

            if (!isAttackOverlap && !isRecordOverlap) break;

            yield return null;
        }

        isPrepare = true;
    }
}
