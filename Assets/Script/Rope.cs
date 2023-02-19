using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int segmentCnt = 30;
    public int constraintLoop = 30;
    public float segmentLength = 0.1f;
    public float ropeWidth = 0.1f;
    public Vector2 gravity = new Vector2(0f, -9.81f);

    [Space(10f)]
    public Transform startTransform;
    public Transform endTransform;

    private List<Segment> segments = new List<Segment>();

    private void Reset()
    {
        TryGetComponent(out lineRenderer);
    }

    private void Awake()
    {
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
        //맨 위(첫번째) 세그먼트만 고정시킴
        segments[0].position = startTransform.position;
        segments[segments.Count - 1].position = endTransform.position;
        
        for(int i=0;i<segments.Count - 1;i++)
        {
            float distance = (segments[i].position - segments[i + 1].position).magnitude;
            float diff = segmentLength - distance;
            Vector2 dir = (segments[i + 1].position - segments[i].position).normalized;

            Vector2 movement = dir * diff;

            if (i == 0) //첫번째 세그먼트 움직이면 안됨. 두번째 세그먼트만 이동
            {
                segments[i + 1].position += movement;
            }
            else if(i < segments.Count - 1)
            {
                segments[i].position -= movement * 0.5f;
                segments[i + 1].position += movement * 0.5f;
            }
        }
    }

    public class Segment
    {
        public Vector2 previousPos;
        public Vector2 position;
        public Vector2 velocity;

        public Segment(Vector2 _position) //리셋
        {
            previousPos = _position;
            position = _position;
            velocity = Vector2.zero;
        }
    }
}
