using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HangingManager : MonoBehaviour
{
    public bool isTodesstrafe;
    public Offender offender;

    private void Awake()
    {
        offender = FindObjectOfType<Offender>();
    }

    public void ConvertScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void EndTodesstrafe()
    {
        offender.SetisPossibleTodesstrafe(false);
    }

    public void Todesstrafe()
    {
        EndTodesstrafe();
        isTodesstrafe = true;
        Debug.Log("사형");
        Debug.Log(isTodesstrafe);

        DestroyAllLineAndWindow();
    }

    public void UnTodesstrafe()
    {
        EndTodesstrafe();
        isTodesstrafe = false;
        Debug.Log("생존");
    }

    public void DestroyAllLineAndWindow()
    {
        StopAllCoroutines(); // 임시 추후 구현 추가될 경우 수정
        foreach (Line line in Line.lineList)
        {
            Destroy(line.lineObject);
            Destroy(line.windowObject);
        }
        Line.lineList.Clear();
    }
}