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
    public TMP_Text lastMent; //지정 해주기
    public ScrollViewController scrollViewController;

    public bool penCursor;
    
    public List<string> currentList= new List<string>();

    void Start()
    {
        original = Resources.Load<Texture2D>("OriginalCursor");
        hangingManager = FindObjectOfType<HangingManager>();
        penCursor = false;
        scrollViewController=FindObjectOfType<ScrollViewController>();
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
                            Debug.Log("위증");

                            lastMent.text = hit.transform.gameObject.GetComponent<ChangeTextTexture>().lastMent + " " + "불일치 -> 위증";
                            if (lastMent.preferredWidth >= 300)
                            {
                                hit.transform.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = hit.transform.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text + "\n" + "<color=#D94242>불일치 -> 위증</color>";
                            }
                            else
                            {
                                hit.transform.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = hit.transform.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text + " " + "<color=#D94242>불일치 -> 위증</color>";
                            }

                            //int selectedIdx = hit.transform.gameObject.transform.GetSiblingIndex();

                            //기존 진술서 데이터와 "불일치~"가 추가된 데이터 저장
                            GameObject contentParent = hit.transform.gameObject.transform.parent.gameObject;
                            int childCnt = contentParent.transform.childCount;
                            for (int i = 0; i < childCnt; i++)
                            {
                                currentList.Add(contentParent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text);
                            }

                            //기존 진술서 텍스트 객체들 삭제//
                            foreach (Transform child in hit.transform.gameObject.transform.parent.transform)
                            {
                                Destroy(child.gameObject);
                            }

                            scrollViewController.MakeMentCangedList(currentList);

                            for (int i = 0; i < childCnt; i++)
                            {
                                contentParent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = currentList[i];
                            }



                        }
                        else // infoError인 경우
                        {
                            Debug.Log("정보오류");
                            lastMent.text = hit.transform.gameObject.GetComponent<ChangeTextTexture>().lastMent + " " + "불일치 -> 정보오류";
                            if (lastMent.preferredWidth >= 300)
                            {
                                hit.transform.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = hit.transform.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text + "\n" + "<color=#D94242>불일치 -> 정보오류</color>";
                            }
                            else
                            {
                                hit.transform.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = hit.transform.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text + " " + "<color=#D94242>불일치 -> 정보오류</color>";
                            }

                            //int selectedIdx = hit.transform.gameObject.transform.GetSiblingIndex();

                            //기존 진술서 데이터와 "불일치~"가 추가된 데이터 저장
                            GameObject contentParent = hit.transform.gameObject.transform.parent.gameObject;
                            int childCnt = contentParent.transform.childCount;
                            for (int i = 0; i < childCnt; i++)
                            {
                                currentList.Add(contentParent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text);
                            }

                            //기존 진술서 텍스트 객체들 삭제//
                            foreach (Transform child in hit.transform.gameObject.transform.parent.transform)
                            {
                                Destroy(child.gameObject);
                            }

                            scrollViewController.MakeMentCangedList(currentList);

                            for (int i = 0; i < childCnt; i++)
                            {
                                contentParent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = currentList[i];
                            }

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


