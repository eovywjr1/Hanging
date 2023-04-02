using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

[AddComponentMenu("UI/DebugTextComponentName", 11)]
public class DominentLogin : MonoBehaviour
{
    public TMP_Text password;

    public float waitSec;
    public float interval;
    public int cnt;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(waitSec);

        for (int i = 0; i < cnt; i++)
        {
            password.text += "*";
            yield return new WaitForSeconds(interval);
        }

        yield return new WaitForSeconds(waitSec);
        SceneManager.LoadScene("StatisticsSceen");
    }
}
