using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour, IListener
{
    private bool isMove, isPossibleMove;
    private float speed = 10f;
    private float _directionY = 1f;
    private float maxPositionY = 0;
    private float downMaxPositionY = -7.2f, upMaxPositionY = 0;

    private void Awake()
    {
        EventManager.instance.addListener("possiblemoveCameraToDesk", this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isPossibleMove)
        {
            moveToDesk(_directionY * -1);

            EventManager.instance.postNotification("dialogEvent", this, "moveCameraToDesk");
        }

        if (isMove)
        {
            transform.Translate(new Vector3(0, _directionY, 0) * speed * Time.deltaTime);
            if ((_directionY == -1 && transform.position.y <= downMaxPositionY) || (_directionY == 1 && transform.position.y >= upMaxPositionY))
            {
                float endY = (_directionY == -1) ? downMaxPositionY : upMaxPositionY;
                transform.position = new Vector3(transform.position.x, endY, transform.position.z);

                isMove = false;
            }
        }
    }
    
    public void moveToDesk(float directionY)
    {
        isMove = true;
        _directionY = directionY;
        maxPositionY = (directionY == 1) ? upMaxPositionY : downMaxPositionY;
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
