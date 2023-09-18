using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using hanging;

public class LineManager : MonoBehaviour
{
    [SerializeField] GameObject linePrefab;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void CreateLine()
    {
        Transform parentTransform;

        if (Line.lineList.Count == 0)
        {
            parentTransform = FindObjectOfType<AttackerMouseMove>().gameObject.transform;
        }
        else
        {
            parentTransform = Line.lineList[Line.lineList.Count - 1].windowObject.transform;
        }

        GameObject line = Instantiate(linePrefab, parentTransform.position, Quaternion.identity, parentTransform);
    }

    private void resetLine()
    {
        Line.lineList.Clear();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        resetLine();
    }
}