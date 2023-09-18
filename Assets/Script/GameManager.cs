using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static int _maxDay = 21;
    public DialogData _dialogData;

    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _dialogData = new DialogData();
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

public class DialogData 
{
    public List<Dictionary<string, List<List<string>>>> compulsoryDialogTable = new List<Dictionary<string, List<List<string>>>>();
    public Dictionary<string, List<List<string>>> situationDialogData;

    public DialogData()
    {
        DialogCSVReader dialogReader = new DialogCSVReader();

        for (int day = 1; day <= GameManager._maxDay; ++day)
        {
            string fileName = day + "DayCompulsoryDialog";
            string filePath = "Assets/Resources/" + fileName + ".csv";
            FileInfo fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists == false)
            {
                Debug.Assert(false, "조민수 comment : " + filePath + "이 없습니다 파일을 추가해주세요.");
                continue;
            }

            compulsoryDialogTable.Add(dialogReader.Read(fileName));
        }

        situationDialogData = dialogReader.Read("SituationDialog");
    }

    public Dictionary<string, List<List<string>>> getCompulsoryDialogTable(int day)
    {
        return compulsoryDialogTable[day - 1];
    }
}
