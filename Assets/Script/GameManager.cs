using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator endGame()
    {
        yield return StartCoroutine(FindObjectOfType<EffectManager>().activeFullillusion());

        SceneManager.LoadScene("GameOver");
        StartCoroutine(exitGame());
    }

    public IEnumerator exitGame()
    {
        yield return new WaitForSecondsRealtime(3f);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
