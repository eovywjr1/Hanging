using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceColorData : MonoBehaviour
{
    [SerializeField] private int hairColor;
    [SerializeField] private int eyeColor;

    [SerializeField] private string []hairColorList = { "검은색", "갈색", "붉은색", "노란색", "흰색" };
    [SerializeField] private string []eyeColorList = { "검은색", "갈색", "푸른색", "초록색" };

    private void Awake()
    {
        // | 흑발 : 0 | 갈발 : 1 | 적발 : 2 | 금발 : 3 | 백발 : 4 | 
        hairColor = Random.Range(0, 5);

        // | 흑안 : 0 | 갈안 : 1 | 벽안 : 2 | 녹안 : 3 |
        eyeColor = Random.Range(0, 4);
    }

    public int getHairColor()   {   return hairColor;   }
    public int getEyeColor()    {   return eyeColor;    }
    public string getHairColorName() {  return hairColorList[hairColor];    }
    public string getEyeColorName() {   return eyeColorList[eyeColor];  }
}
