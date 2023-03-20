using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AddDeclaration : MonoBehaviour
{
    public GameObject declaration;
    

    public void AddLine()
    {
        Instantiate(declaration);
    }
}
