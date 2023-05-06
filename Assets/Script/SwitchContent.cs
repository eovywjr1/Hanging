using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchContent : MonoBehaviour
{
    public GameObject content1;
    public GameObject content2;

    private bool isContent1Active = true;

    public void SwitchButton()
    {
        if(isContent1Active)
        {
            content1.SetActive(false);
            content2.SetActive(true);
        }
        else
        {
            content1.SetActive(true);
            content2.SetActive(false);
        }

        isContent1Active = !isContent1Active;
    }
}
