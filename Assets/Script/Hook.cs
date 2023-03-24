using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public GameObject linkPrefab;
    public GameObject connectedObj;
    public int links;
    public float upperAngle = 10f;

    private void Start()
    {
        Rigidbody2D prevLink = GetComponent<Rigidbody2D>();

        for(int i=0;i<links;i++)
        {
            GameObject link = Instantiate(linkPrefab, transform);
            link.GetComponent<HingeJoint2D>().connectedBody = prevLink;
            prevLink = link.GetComponent<Rigidbody2D>();

            if(i == links - 1)
            {
                connectedObj.transform.localEulerAngles = new Vector3(0, 0, 0);
                SetConnectedObject(connectedObj, prevLink);
            }
        }
    }

    void SetConnectedObject(GameObject obj, Rigidbody2D rb)
    {
        HingeJoint2D ObjHingeJoint = obj.AddComponent<HingeJoint2D>();
        JointAngleLimits2D ObjHingeLimits = ObjHingeJoint.limits;

        ObjHingeJoint.connectedBody = rb;
        ObjHingeJoint.autoConfigureConnectedAnchor = false;
        ObjHingeJoint.anchor = new Vector2(0, -2);
        ObjHingeJoint.connectedAnchor = Vector2.zero;

        ObjHingeLimits.max = upperAngle;
        ObjHingeJoint.limits = ObjHingeLimits;
    }
}
