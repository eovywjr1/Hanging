using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private GameObject illusionFullScreen;

    private void Awake()
    {
        illusionFullScreen = GameObject.Find("etc_effect").transform.Find("illusion_FullScreen").gameObject;
        Debug.Assert(illusionFullScreen, "조민수 : illusion_FullScreen 오브젝트가 없습니다.");
    }

    public IEnumerator activeFullillusion()
    {
        illusionFullScreen.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);
    }
}
