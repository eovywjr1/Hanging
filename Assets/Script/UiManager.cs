using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public GameObject GuideButton;

    private void Awake()
    {
        GuideButton.SetActive(false);
    }

    private void Update()
    {
        guideOn();
    }

    private void guideOn()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (GuideButton.activeSelf == false)
            {
                GuideButton.SetActive(true);

            }
            else
            {
                GuideButton.SetActive(false);
            }
        }
    }
}
