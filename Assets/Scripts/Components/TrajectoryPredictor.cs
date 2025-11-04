using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPredictor : MonoBehaviour
{
    [Header("발사 관련 설정")]
    public Transform shootPoint;     // 포탄 발사 위치
    public float power = 5f;        // 발사 세기

    [Header("물리 설정")]
    public int maxStep = 10;
    public float timeStep = 0.1f;    // 시뮬레이션 간격
    public float gravityScale = 0.8f;  // 중력 배율

    [Header("충돌 감지")]
    public LayerMask collisionMask;
    public float radius = 0.1f;

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    public void ShowTrajectory(Vector2 direction)
    {
        Vector2 startPos = shootPoint.position;
        Vector2 velocity = direction.normalized * power;
        Vector2 gravity = Physics2D.gravity * gravityScale;

        List<Vector3> points = new List<Vector3>();
        Vector2 currentPos = startPos;

        for (int i = 0; i < maxStep; i++)
        {
            float t = i * timeStep;
            Vector2 pos = startPos + velocity * t + 0.5f * gravity * t * t;

            RaycastHit2D hit = Physics2D.CircleCast(
                currentPos, radius, pos - currentPos,
                Vector2.Distance(currentPos, pos), collisionMask);

            points.Add(pos);

            if(hit.collider != null)
            {
                points[points.Count - 1] = hit.point;
                break;
            }
        }

        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
        line.enabled = true;
    }

    public void HideTrajectory()
    {
        line.enabled = false;
    }
}
