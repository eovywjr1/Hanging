using UnityEngine;
using UnityEngine.UI;

public class MyScrollView : MonoBehaviour
{
    public RectTransform content;

    private Vector2 _scrollPosition = Vector2.zero;

    void OnGUI()
    {
        _scrollPosition = GUI.BeginScrollView(new Rect(0, 0, Screen.width, Screen.height), _scrollPosition, content.rect);
        // content를 그리는 코드
        GUI.EndScrollView();
    }
}
