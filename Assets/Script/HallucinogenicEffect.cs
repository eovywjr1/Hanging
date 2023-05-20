using Kino;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HallucinogenicEffect : MonoBehaviour
{
    public GameObject borderImage;
    public GameObject borderImage2;
    public GameObject borderImage3;

    public GameObject prefab_eye;
    public GameObject prefab_spin;

    private GameObject eye;
    private GameObject spin;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.GetChild(0).transform.gameObject.SetActive(false);
        gameObject.transform.GetChild(1).transform.gameObject.SetActive(false);
        gameObject.transform.GetChild(2).transform.gameObject.SetActive(false);

        ManageHallucination();

    }

    private void ManageHallucination()//일차 변환 시 마다 해당 함수 실행 필요
    {
        

        switch (GetStep(HangingManager.day))
        {
            case 1:
                gameObject.transform.GetChild(0).transform.gameObject.SetActive(true);
                gameObject.transform.GetChild(1).transform.gameObject.SetActive(false);
                gameObject.transform.GetChild(2).transform.gameObject.SetActive(false);
                break;
            case 2:
                gameObject.transform.GetChild(1).transform.gameObject.SetActive(true);
                gameObject.transform.GetChild(0).transform.gameObject.SetActive(false);
                gameObject.transform.GetChild(2).transform.gameObject.SetActive(false);
                break;
            case 3:
                gameObject.transform.GetChild(2).transform.gameObject.SetActive(true);
                gameObject.transform.GetChild(0).transform.gameObject.SetActive(false);
                gameObject.transform.GetChild(1).transform.gameObject.SetActive(false);
                break;
        }

   
    }

    IEnumerator ObstructionWindow(float time,int probability) //time: 창 뜨기 전 대기 시간, probabillity: 창 뜰 확률
    {
        Debug.Log("시작함");
        while (true)
        {
            yield return new WaitForSecondsRealtime(time);

            if (Random.Range(1, 101) <= probability)
            {
                eye.transform.gameObject.SetActive(true); //눈 이미지
                spin.transform.gameObject.SetActive(true); //분홍색 이미지
  
                yield return new WaitForSecondsRealtime(3f); //시야 방해 창 출력 시간

                eye.transform.gameObject.SetActive(false); //눈 이미지
                spin.transform.gameObject.SetActive(false); //분홍색 이미지

                
            }
        }
    }

    public void StartObstructionWindow(GameObject window)
    {
        //정보창 주변 위치로 생성
        Instantiate(prefab_spin, new Vector2(window.transform.position.x + Random.Range(-1.0f, 1.0f), window.transform.position.y + Random.Range(-1.0f, 1.0f)), Quaternion.identity);
        Instantiate(prefab_eye, new Vector2(window.transform.position.x + Random.Range(-1.0f, 1.0f), window.transform.position.y + Random.Range(-1.0f, 1.0f)) , Quaternion.identity);

        eye = GameObject.Find("spin(Clone)");
        spin = GameObject.Find("etc_eye_hallucination(Clone)");

        eye.SetActive(false);
        spin.SetActive(false);


        //부모 윈도우 창으로 설정해 윈도우 이동 시 환각도 같이 이동하게 함
        eye.transform.parent= window.transform;
        spin.transform.parent= window.transform;

        switch (GetStep(HangingManager.day))
        {
            case 1:
                StartCoroutine(ObstructionWindow(10f, 100));
                break;
            case 2:
                StartCoroutine(ObstructionWindow(10f, 100));
                break;
            case 3:
                StartCoroutine(ObstructionWindow(7f, 100));
                break;
        }
    }

    private int GetStep(int day)
    {
        int step=0;
        switch (day)
        {
            case 1:
                step = 0;
                break;
            case 2:
                step = 0;
                break;

            case 3:
                step = 1;
                break;
            case 4:
                step = 1;
                break;

            case 5:
                step = 2;
                break;
            case 6:
                step = 2;
                break;

            case 7:
                step = 3;
                break;
        }
        return step;
    }
}
