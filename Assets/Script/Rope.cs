using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rope : MonoBehaviour
{
    public prisoner prisoner;
    private Rigidbody2D prisonerRigidbody;

    public LineRenderer lineRenderer;
    public int segmentCnt = 50;
    public int constraintLoop = 50;
    public float segmentLength = 0.1f;
    public float ropeWidth = 0.1f;
    public Vector2 gravity = new Vector2(0f, -9.81f);

    [Space(10f)]
    Transform startTransform;
    public Transform endTransform;

    private List<Segment> segments = new List<Segment>();

    private bool isPossibleCut = false;
    private bool isCut = false;

    private void Reset()
    {
        TryGetComponent(out lineRenderer);
    }

    private void Awake()
    {
        prisonerRigidbody = prisoner.GetComponent<Rigidbody2D>();
        startTransform = GameObject.Find("RopeStartPoint").transform;

        Vector2 segmentPos = startTransform.position;
        for(int i=0;i<segmentCnt;i++)
        {
            segments.Add(new Segment(segmentPos));
            segmentPos.y -= segmentLength;
        }
    }

    private void FixedUpdate()
    {
        UpdateSegments();
        for(int i=0;i<constraintLoop;i++)
        {
            ApplyConstraint();
        }
        DrawRope();
    }

    public void SetCutPossible(bool _isCutPossible)
    {
        isPossibleCut = _isCutPossible;
    }

    private void Update()
    {
        if ((isPossibleCut == false) || (isCut))
            return;

        if ((Input.GetMouseButtonDown(0) == false) && (Input.GetMouseButtonDown(1) == false))
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float cutRange = 2.0f;  //밧줄 자를 수 있는 반경 범위
        float distanceToStart = Vector2.Distance(startTransform.position, mousePos);

        if (cutRange < distanceToStart)
            return;

        int closestSegmentIndex = FindClosestSegmentIndex(mousePos);

        endTransform = null;
        if (closestSegmentIndex != -1)
        {
            for (int i = closestSegmentIndex; i < segments.Count; i++)
            {
                segments.RemoveAt(i);
            }
        }

        prisonerRigidbody.bodyType = RigidbodyType2D.Dynamic;

        cutRope();
    }

    private int FindClosestSegmentIndex(Vector2 mousePos)
    {
        float minDistance = Mathf.Infinity;
        int closestIndex = -1;

        for (int i = 0; i < segments.Count; i++)
        {
            float distance = (mousePos - segments[i].position).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }


    private void DrawRope()
    {
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;

        Vector3[] segmentPos = new Vector3[segments.Count];
        for(int i=0;i<segments.Count;i++)
        {
            segmentPos[i] = segments[i].position;
        }
        lineRenderer.positionCount = segmentPos.Length;
        lineRenderer.SetPositions(segmentPos);
    }

    private void UpdateSegments()
    {
        for(int i=0;i<segments.Count;i++)
        {
            segments[i].velocity = segments[i].position - segments[i].previousPos;
            segments[i].previousPos = segments[i].position;
            segments[i].position += gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
            segments[i].position += segments[i].velocity;
        }
    }


    private void ApplyConstraint()
    {
        int lastIdx = segments.Count - 1;

        segments[0].position = startTransform.position;

        if (endTransform)
        {
            segments[lastIdx].position = endTransform.position;
        }

        for(int i=0;i< lastIdx;i++)
        {
            float distance = (segments[i].position - segments[i + 1].position).magnitude;
            float diff = segmentLength - distance;
            Vector2 dir = (segments[i + 1].position - segments[i].position).normalized;

            Vector2 movement = dir * diff;

            if (i == 0)
            {
                segments[i + 1].position += movement;
            }
            else if(i < lastIdx )
            {
                segments[i].position -= movement * 0.5f;
                segments[i + 1].position += movement * 0.5f;
            }
        }
    }

    private void cutRope()
    {
        prisoner.isCutRope = true;
        isCut = true;

        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            EventManager.instance.postNotification("dialogEvent", this, UnityEngine.Random.Range(21, 28));

            StartCoroutine(DelayedEvents());
        }
    }

    private IEnumerator DelayedEvents()
    {
        yield return new WaitForSeconds(3.1f);

        EventManager.instance.postNotification("amnesty", this, null);
        EventManager.instance.postNotification("dialogEvent", this, "amnesty");
    }

    public class Segment
    {
        public Vector2 previousPos;
        public Vector2 position;
        public Vector2 velocity;

        public Segment(Vector2 _position)
        {
            previousPos = _position;
            position = _position;
            velocity = Vector2.zero;
        }
    }
}
