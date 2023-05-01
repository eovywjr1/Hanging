using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
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

    [SerializeField]
    bool isCutPossible = true;

    private void Reset()
    {
        TryGetComponent(out lineRenderer);
    }

    private void Awake()
    {
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
        isCutPossible = _isCutPossible;
    }

    private void Update()
    {
        if(isCutPossible)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int closestSegmentIndex = FindClosestSegmentIndex(mousePos);
                Debug.Log("���� ���׸�Ʈ �ε��� : " + closestSegmentIndex);

                endTransform = null;
                if (closestSegmentIndex != -1)
                {
                    for (int i = closestSegmentIndex; i < segments.Count; i++)
                    {
                        segments.RemoveAt(i);
                    }
                }
            }
        }
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
        int lastIdx = segments.Count;

        //�� ��(ù��°) ���׸�Ʈ�� ������Ŵ
        segments[0].position = startTransform.position;

        if (endTransform)
        {
            segments[segments.Count - 1].position = endTransform.position;
            lastIdx -= 1;
        }

        for(int i=0;i<segments.Count - 1;i++)
        {
            float distance = (segments[i].position - segments[i + 1].position).magnitude;
            float diff = segmentLength - distance;
            Vector2 dir = (segments[i + 1].position - segments[i].position).normalized;

            Vector2 movement = dir * diff;

            if (i == 0) //ù��° ���׸�Ʈ �����̸� �ȵ�. �ι�° ���׸�Ʈ�� �̵�
            {
                segments[i + 1].position += movement;
            }
            else if(i < lastIdx)
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

        public Segment(Vector2 _position) //����
        {
            previousPos = _position;
            position = _position;
            velocity = Vector2.zero;
        }
    }
}
