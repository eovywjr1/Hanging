using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[AddComponentMenu("UI/DebugTextComponentName", 11)]
public class UIDominantLogin : MonoBehaviour
{
    public TMP_Text password;

    private float waitSec = 1.25f;
    private float interval = 0.2f;
    private int cnt = 18;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(waitSec);

        for (int i = 0; i < cnt; i++)
        {
            password.text += "*";
            yield return new WaitForSeconds(interval);
        }

        yield return new WaitForSeconds(waitSec);

        FindObjectOfType<UiManager>().showStatistics();

        Destroy(this);
    }
}
