using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TortureManage : MonoBehaviour
{
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject lv1_Button;
    [SerializeField] GameObject lv2_Button;
    [SerializeField] GameObject lv3_Button;
    [SerializeField] GameObject heartRate;

    WorkManager workManager;
    public GameObject prisoner;
    public prisoner prisonerScript;

    HeartRate heartRateSC;

    public int tortureCount = 0;
    public bool tortureStatus = false;
    float probabilityOfDeath = 0;   //사망확률 1 되면 사망 (추후 구현 필요)

    private void Awake()
    {
        workManager = FindObjectOfType<WorkManager>();
        
        startButton.SetActive(true);
        lv1_Button.SetActive(false);
        lv2_Button.SetActive(false);
        lv3_Button.SetActive(false);
        heartRate.SetActive(false);
    }

    private void Update()
    {
        if (tortureCount >= 2)
        {
            workManager.tortureCondition = false;
        }

        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;

        // 고문 작업 가능 할 때, 고문버튼이 아닌 다른 부분 클릭하면 고문시작 버튼만 나타나게 바뀜
        if(clickedObject != lv1_Button.gameObject &&
            clickedObject != lv2_Button.gameObject &&
            clickedObject != lv3_Button.gameObject &&
            clickedObject != startButton.gameObject)
        {
            startButton.SetActive(true);
            lv1_Button.SetActive(false);
            lv2_Button.SetActive(false);
            lv3_Button.SetActive(false);
            heartRate.SetActive(false);
        }
    }

    public void StartTorture()
    {
        lv1_Button.SetActive(true);
        lv2_Button.SetActive(true);
        lv3_Button.SetActive(true);
        heartRate.SetActive(true);
        startButton.SetActive(false);

        prisoner = GameObject.Find("Prisoner(Clone)");
        prisonerScript = prisoner.GetComponentInChildren<prisoner>();

        heartRateSC = FindObjectOfType<HeartRate>();
    }

    public void Torture_Lv1()
    {
        prisonerScript.isPlayingTortureAnim = true;
        prisonerScript.PlayTortureAnimation(1);
        prisonerScript.StopTortureAnimation(1);

        tortureStatus = true;
        tortureCount++;

        probabilityOfDeath = 0.15f;
    }

    public void Torture_Lv2()
    {
        prisonerScript.isPlayingTortureAnim = true;
        prisonerScript.PlayTortureAnimation(2);
        prisonerScript.StopTortureAnimation(2);

        tortureStatus = true;
        tortureCount++;

        probabilityOfDeath = 0.25f;
    }

    public void Torture_Lv3()
    {
        prisonerScript.isPlayingTortureAnim = true;
        prisonerScript.PlayTortureAnimation(3);
        prisonerScript.StopTortureAnimation(3);

        tortureStatus = true;
        tortureCount++;

        probabilityOfDeath = 0.45f;
    }
}
