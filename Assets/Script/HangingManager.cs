using Kino;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HangingManager : MonoBehaviour
{
    public bool isTodesstrafe;
    public AttackerMouseMove attackerMouseMove;
    public AttackerInfo attackerInfo;
    public static int day = 2, attackerCount = 1;
    private AnalogGlitch analogGlitch;

    private void Awake()
    {
        attackerMouseMove = FindObjectOfType<AttackerMouseMove>();
        attackerInfo = FindObjectOfType<AttackerInfo>();
        analogGlitch = FindObjectOfType<AnalogGlitch>();
    }

    public void EndTodesstrafe()
    {
        attackerMouseMove.SetisPossibleTodesstrafe(false);
        DestroyAllLineAndWindow();

        isTodesstrafe = true;
    }

    public void Todesstrafe()
    {
        EndTodesstrafe();

        //사형 판별//
        if (DistinguishTodesstrafe(0)) Debug.Log("True");
        else Debug.Log("False");
        Debug.Log("사형");
    }

    public void UnTodesstrafe()
    {
        EndTodesstrafe();

        //사형 판별//
        if (DistinguishTodesstrafe(1)) Debug.Log("True");
        else Debug.Log("False");
        Debug.Log("생존");
    }

    private bool DistinguishTodesstrafe(int mode)
    {
        if (mode == attackerInfo.recordData.isHanging)
        {
            NextAttacker();
            return true;
        }
        else
        {
            StartCoroutine(StartGlitch());

            return false;
        }
    }

    private void DestroyAllLineAndWindow()
    {
        attackerMouseMove.StopAllCoroutines();
        foreach (Line line in Line.lineList)
        {
            Destroy(line.lineObject);
            Destroy(line.windowObject);
        }
        Line.lineList.Clear();
    }

    void NextAttacker()
    {
        attackerCount++;

        SceneManager.LoadScene("Merge");
    }
    IEnumerator StartGlitch()
    {
        analogGlitch._isGlitch = true;

        yield return new WaitForSecondsRealtime(0.75f);

        analogGlitch._isGlitch = false;
        NextAttacker();
    }
}