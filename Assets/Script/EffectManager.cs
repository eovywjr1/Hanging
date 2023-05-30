using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour, IListener
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
        yield return new WaitForSecondsRealtime(5f);
        illusionFullScreen.SetActive(false);
    }

    public void OnEvent(string eventType, Component sender, object parameter = null)
    {
        switch (eventType)
        {
            case "activeFullillusion":
                StartCoroutine(activeFullillusion());
                break;
        }
    }
}
