

using UnityEngine;
using System.Collections;

public class Noise : MonoBehaviour
{

    public Color edgeColor = Color.white;
    public float edgeWidth = 0.01f;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Material mat = new Material(Shader.Find("Custom/EdgeEffect"));
        mat.SetColor("_EdgeColor", edgeColor);
        mat.SetFloat("_EdgeWidth", edgeWidth);
        Graphics.Blit(src, dest, mat);
    }

}