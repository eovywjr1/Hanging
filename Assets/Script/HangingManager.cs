using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HangingManager : MonoBehaviour
{
    public static bool isTodesstrafe;
    public HangingMove hangingMove;

    private void Start()
    {
        hangingMove = FindObjectOfType<HangingMove>();
    }

    public void ConvertScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}