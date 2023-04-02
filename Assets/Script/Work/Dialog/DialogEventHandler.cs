using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogEventHandler : MonoBehaviour
{
    public bool ActiveGuide()
    {
        if (Input.GetKeyDown(KeyCode.G))
            return true;

        return false;
    }
}
