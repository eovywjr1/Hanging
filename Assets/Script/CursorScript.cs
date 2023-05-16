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
    public TMP_Text lastMent; //���� ���ֱ�
    public ScrollViewController scrollViewController;
    public GameObject padStatement;
    public GameObject penButton;
    public GameObject padStatementText;

    public bool penCursor;
    
    public List<string> currentList= new List<string>();
    public List<bool> currentClick= new List<bool>();

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

                if (hit.transform.gameObject.name == "staButton")
                {
                    if (padStatement.activeSelf == false)
                    { 
                        padStatement.SetActive(true);
                        penButton.SetActive(true);
                        padStatementText.transform.position += new Vector3(0, 0, 10);

                    }
                    else
                    { 
                        padStatement.SetActive(false);
                        penButton.SetActive(false); 
                        padStatementText.transform.position += new Vector3(0, 0, -10);
                    }
                }

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
                        Debug.Log("������ ������ �ٸ�");

                        if (hit.transform.gameObject.GetComponent<ChangeTextTexture>().lieORinfoErrorValue == 1) // lie�� ���
                        {
                            Debug.Log("����");
                            AddDiscordMent("����ġ -> ����", hit.transform.gameObject);
                        }
                        else // infoError�� ���
                        {
                            Debug.Log("��������");
                            AddDiscordMent("����ġ -> ����ġ", hit.transform.gameObject);
                        }

                        hit.transform.gameObject.GetComponent<ChangeTextTexture>().afterClick = true; //Ŭ���� ��ü afterClick true

                        SaveDiscordMent(hit.transform.gameObject); //����ġ ���� �߰��� ������, afterClick ������ ����

                        RemoveTextObject(hit.transform.gameObject); //Ŭ�� ���� �ؽ�Ʈ ������Ʈ�� ���� �ʿ� �����Ƿ� ����

                        scrollViewController.MakeMentCangedList(currentList, currentClick); //���ο� �ؽ�Ʈ ������Ʈ ����
                    }
                    else
                    {
                        if (hangingManager.isStatementWrongProcess == false)
                        {
                            Debug.Log("������ ������ ����");
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
        if (penCursor)
        {
            Cursor.SetCursor(changed, Vector2.zero, CursorMode.Auto);
        }
        
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(original, Vector2.zero, CursorMode.Auto);
    }

    private void AddDiscordMent(string str, GameObject hit)
    {
        lastMent.text = hit.transform.gameObject.GetComponent<ChangeTextTexture>().lastMent + " " + "����ġ -> ����";
        if (lastMent.preferredWidth >= 300)
        {
            hit.transform.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text += "\n" + "<color=#D94242>����ġ -> ����</color>";
        }
        else
        {
            hit.transform.gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text += " " + "<color=#D94242>����ġ -> ����</color>";
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


