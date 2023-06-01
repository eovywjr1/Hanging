using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour, IListener
{
    private bool isMove, isPossibleMove;
    private float speed = 10f;
    private float directionY = 1f;
    private float maxPositionY = 0;
    private float downMaxPositionY = -7.2f, upMaxPositionY = 0;

    private void Start()
    {
        EventManager.instance.addListener("possiblemoveCameraToDesk", this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isPossibleMove)
        {
            directionY *= -1f;
            maxPositionY = (maxPositionY == downMaxPositionY) ? upMaxPositionY : downMaxPositionY;
            isMove = true;

            EventManager.instance.postNotification("dialogEvent", this, "moveCameraToDesk");
        }

        if (isMove)
        {
            transform.Translate(new Vector3(0, directionY, 0) * speed * Time.deltaTime);
            if ((directionY == -1 && transform.position.y <= downMaxPositionY) || (directionY == 1 && transform.position.y >= upMaxPositionY))
            {
                float endY = (directionY == -1) ? downMaxPositionY : upMaxPositionY;
                transform.position = new Vector3(transform.position.x, endY, transform.position.z);

                isMove = false;
            }
        }
    }

    public void OnEvent(string eventType, Component sender, object parameter = null)
    {
        switch (eventType)
        {
            case "possiblemoveCameraToDesk":
                isPossibleMove = true;
                break;
        }
    }
}
