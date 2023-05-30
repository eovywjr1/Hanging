using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Burst.CompilerServices;

public class CursorScript : MonoBehaviour
{
    Texture2D original;
    HangingManager hangingManager;
    [SerializeField] Texture2D changed;
    public TMP_Text lastMent; //지정 해주기
    public ScrollViewController scrollViewController;
    public GameObject padStatement;
    //public GameObject penButton;
    public GameObject padStatementText;

    public bool penCursor;
    
    public List<string> currentList= new List<string>();
    public List<bool> currentClick= new List<bool>();
    public List<bool> compareMentBoolLIst= new List<bool>();

    void Start()
    {
        original = Resources.Load<Texture2D>("OriginalCursor");
        hangingManager = FindObjectOfType<HangingManager>();
        penCursor = false;
        scrollViewController=FindObjectOfType<ScrollViewController>();

        padStatement.SetActive(false);
        //penButton.SetActive(false);
        padStatementText.transform.position += new Vector3(0, 0, -11);
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

                if (hit.transform.gameObject.name == "staButton")
                {
                    if (padStatement.activeSelf == false)
                    { 
                        padStatement.SetActive(true);
                        penCursor = true;
                        //penButton.SetActive(true);
                        padStatementText.transform.position += new Vector3(0, 0, 11);

                    }
                    else
                    { 
                        padStatement.SetActive(false);
                        //penButton.SetActive(false);
                        penCursor = false;
                        padStatementText.transform.position += new Vector3(0, 0, -11);
                    }
                }

               
                if (hit.transform.gameObject.CompareTag("statement") && penCursor == true && hit.transform.gameObject.GetComponent<ChangeTextTexture>().afterClick == false)
                {
                    if (!hit.transform.gameObject.GetComponent<ChangeTextTexture>().mentTureORFalse)
                    {
                        Debug.Log("진술서 내용이 다름");

                        if (hit.transform.gameObject.GetComponent<ChangeTextTexture>().lieORinfoErrorValue == 1) // lie인 경우
                        {
                            Debug.Log("위증");
                            AddDiscordMent("불일치 -> 위증", hit.transform.gameObject);
                        }
                        else // infoError인 경우
                        {
                            Debug.Log("정보오류");
                            AddDiscordMent("불일치 -> 불일치", hit.transform.gameObject);
                        }

                        hit.transform.gameObject.GetComponent<ChangeTextTexture>().afterClick = true; //클릭한 객체 afterClick true

                        SaveDiscordMent(hit.transform.gameObject); //불일치 내용 추가한 데이터, afterClick 데이터 저장

                        RemoveTextObject(hit.transform.gameObject); //클릭 전의 텍스트 오브젝트는 이제 필요 없으므로 삭제

                        scrollViewController.MakeMentCangedList(currentList, currentClick, compareMentBoolLIst); //새로운 텍스트 오브젝트 생성
                    }
                    else
                    {
                        if (hangingManager.isStatementWrongProcess == false)
                        {
                            Debug.Log("진술서 내용이 같음");
                            StartCoroutine(hangingManager.StartStateWrong());

                            hit.transform.gameObject.GetComponent<ChangeTextTexture>().afterClick = true;
                        }
                    }
                }
            }
        }
    }

    private void OnMouseEnter()
    {
            Cursor.SetCursor(changed, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(original, Vector2.zero, CursorMode.Auto);
    }

    private void AddDiscordMent(string str, GameObject hit)
    {
        lastMent.text = hit.transform.gameObject.GetComponent<ChangeTextTexture>().lastMent + " " + "불일치 -> 위증";
        if (lastMent.preferredWidth >= 300)
        {
            hit.transform.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text += "\n" + "<color=#D94242>불일치 -> 위증</color>";
        }
        else
        {
            hit.transform.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text += " " + "<color=#D94242>불일치 -> 위증</color>";
        }
    }

    private void SaveDiscordMent(GameObject hit)
    {
        GameObject contentParent = hit.transform.gameObject.transform.parent.gameObject;
        int childCnt = contentParent.transform.childCount;
        for (int i = 0; i < childCnt; i++)
        {
            currentList.Add(contentParent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text);
            currentClick.Add(contentParent.transform.GetChild(i).GetComponent<ChangeTextTexture>().afterClick);
            compareMentBoolLIst.Add(contentParent.transform.GetChild(i).GetComponent<ChangeTextTexture>().mentTureORFalse);
        }
    }

    private void RemoveTextObject(GameObject hit)
    {
        foreach (Transform child in hit.transform.gameObject.transform.parent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}


