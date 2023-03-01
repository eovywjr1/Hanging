using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class Swing : MonoBehaviour
{
    public float circleR; //반지름
    public float objSpeed; //원운동 속도
    public GameObject point;
    public float runningTime;

    private void Start()
    {
        circleR= Random.Range(0.1f, 0.3f);
        objSpeed = Random.Range(1.5f, 3.3f);
        var x= Random.Range(0.01f, 0.05f);
        var y= Random.Range(0.01f, 0.05f);

        gameObject.transform.parent.transform.position = gameObject.transform.parent.transform.position + new Vector3(x, y);
    }

    void Update()
    {
        runningTime += Time.deltaTime * objSpeed;

        //var rad = Mathf.Deg2Rad * (runningTime);
        var x = circleR * Mathf.Sin(runningTime);
        var y = circleR * Mathf.Cos(runningTime);
        Debug.Log("x:" + x);
        Debug.Log("y:" + y);
        gameObject.transform.position = point.GetComponent<Transform>().position + new Vector3(x, y);

    }
}

