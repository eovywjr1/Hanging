using Kino;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallucinogenicEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    IEnumerator StartBorderEffectStep()
    {
        yield return new WaitForSecondsRealtime(0.75f);

    }
}
