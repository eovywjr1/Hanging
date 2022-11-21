using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HangingManager : MonoBehaviour
{
    public bool isTodesstrafe;
    public HangingMove hangingMove;

    private void Start()
    {
        hangingMove = FindObjectOfType<HangingMove>();
    }

    public void ConvertScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void EndTodesstrafe()
    {
        hangingMove.SetisPossibleTodesstrafe(false);
    }

    public void Todesstrafe()
    {
        EndTodesstrafe();
        isTodesstrafe = true;
        Debug.Log("사형");
        Debug.Log(isTodesstrafe);
    }

    public void UnTodesstrafe()
    {
        EndTodesstrafe();
        isTodesstrafe = false;
        Debug.Log("생존");
    }
}