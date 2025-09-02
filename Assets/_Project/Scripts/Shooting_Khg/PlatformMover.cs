
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [Header("경로 설정")]
    [Tooltip("플랫폼이 이동할 웨이포인트(Transform) 목록")]
    public List<Transform> waypoints;

    [Header("움직임 설정")]
    [Tooltip("플랫폼의 이동 속도")]
    public float speed = 2.0f;

    [Tooltip("경로의 끝에 도달했을 때의 행동 방식 (Loop: 처음으로, PingPong: 역방향으로)")]
    public MovementType movementType = MovementType.Loop;

    private int currentWaypointIndex = 0;
    private bool movingForward = true;

    public enum MovementType
    {
        Loop,
        PingPong
    }

    void Start()
    {
        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("웨이포인트가 설정되지 않았습니다. PlatformMover를 비활성화합니다.", this);
            enabled = false;
        }
    }

    void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        if (waypoints.Count == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.01f)
        {
            UpdateWaypointIndex();
        }
    }

    private void UpdateWaypointIndex()
    {
        if (movementType == MovementType.Loop)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }
        else if (movementType == MovementType.PingPong)
        {
            if (movingForward)
            {
                if (currentWaypointIndex >= waypoints.Count - 1)
                {
                    movingForward = false;
                    currentWaypointIndex--;
                }
                else
                {
                    currentWaypointIndex++;
                }
            }
            else
            {
                if (currentWaypointIndex <= 0)
                {
                    movingForward = true;
                    currentWaypointIndex++;
                }
                else
                {
                    currentWaypointIndex--;
                }
            }
            currentWaypointIndex = Mathf.Clamp(currentWaypointIndex, 0, waypoints.Count - 1);
        }
    }
}
