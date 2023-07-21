using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextInformationOfFaceColor : MonoBehaviour
{
    [SerializeField] TMP_Text information;
    [SerializeField] FaceColorData faceColorData;

    void Start()
    {
        information.text = "머리카락 : " + faceColorData.getHairColorName() + "\n";
        information.text += "눈동자 : " + faceColorData.getEyeColorName();
    }
}
