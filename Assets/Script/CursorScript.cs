using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CursorScript : MonoBehaviour
{
    Texture2D original;
    HangingManager hangingManager;
    [SerializeField] Texture2D changed;

    public bool penCursor;

    void Start()
    {
        original = Resources.Load<Texture2D>("OriginalCursor");
        hangingManager = FindObjectOfType<HangingManager>();
        penCursor = false;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if(hit.collider != null)
            {
                GameObject clickObj = hit.transform.gameObject;
                Debug.Log(clickObj.name);

                if (hit.transform.gameObject.name == "penButton")
                {
                    if (!penCursor)
                        penCursor = true;
                    else
                        penCursor = false;
                }
                if (hit.transform.gameObject.CompareTag("statement") && penCursor==true && hit.transform.gameObject.GetComponent<ChangeTextTexture>().afterClick == false)
                {
                    if (!hit.transform.gameObject.GetComponent<ChangeTextTexture>().mentTureORFalse)
                    {
                        Debug.Log("진술서 내용이 다름");
                        if (hit.transform.gameObject.GetComponent<ChangeTextTexture>().lieORinfoErrorValue == 1) // lie인 경우
                        {
                            Debug.Log("위증입니다");
                            /*
                            txtALlTmp.text = addStr + "불일치 -> 위증";
                            if (txtALlTmp.preferredWidth >= 300)
                            {
                                //위증 글씨만 아래줄로
                                resultStr = resultStr + System.Environment.NewLine + "불일치 -> 위증";
                            }
                            else
                            {
                                //위증 글씨 그냥 추가
                                resultStr = resultStr + "불일치 -> 위증";
                            }
                            */

                        }
                        else // infoError인 경우
                        {
                            Debug.Log("정보오류입니다");
                        }

                        //진술서 다른 내용 색 변경
                        hit.transform.gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color32(217,66, 66,255);
                        hit.transform.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().color = Color.white;
                        hit.transform.gameObject.GetComponent<ChangeTextTexture>().afterClick = true;
                    }
                    else
                    {
                        Debug.Log("진술서 내용이 같음");
                        StartCoroutine(hangingManager.StartStateWrong());

                        hit.transform.gameObject.GetComponent<ChangeTextTexture>().afterClick = true;


                    }
                    
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        if (penCursor)
        {
            Cursor.SetCursor(changed, Vector2.zero, CursorMode.Auto);
        }
        
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(original, Vector2.zero, CursorMode.Auto);
    }
}


