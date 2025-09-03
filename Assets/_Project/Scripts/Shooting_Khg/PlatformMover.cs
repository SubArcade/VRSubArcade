using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [Header("경로 설정")]
    [Tooltip("플랫폼이 이동할 웨이포인트 목록. 비워두면 아래의 둥실둥실 모드로 작동합니다.")]
    public List<Transform> waypoints;

    [Header("경로 움직임 설정 (웨이포인트 있을 시)")]
    [Tooltip("플랫폼의 이동 속도")]
    public float speed = 2.0f;
    [Tooltip("경로 끝 도달 시 행동 방식 (Loop: 처음으로, PingPong: 역방향으로)")]
    public MovementType movementType = MovementType.Loop;

    [Header("둥실둥실 움직임 (웨이포인트 없을 시)")]
    [Tooltip("위아래로 움직이는 높이")]
    public float floatHeight = 0.3f;
    [Tooltip("위아래로 움직이는 속도")]
    public float floatSpeed = 1f;

    private int currentWaypointIndex = 0;
    private bool movingForward = true;
    private bool useFloatingBehavior = false;
    private Vector3 floatStartPosition;

    public enum MovementType
    {
        Loop,
        PingPong
    }

    void Start()
    {
        // 웨이포인트가 없으면 둥실둥실 모드를 활성화합니다.
        if (waypoints == null || waypoints.Count == 0)
        {
            useFloatingBehavior = true;
            floatStartPosition = transform.position;
        }
    }

    void Update()
    {
        // 모드에 따라 다른 움직임 로직을 실행합니다.
        if (useFloatingBehavior)
        {
            FloatPlatform();
        }
        else
        {
            MovePlatformAlongWaypoints();
        }
    }

    /// <summary>
    /// 제자리에서 위아래로 둥실둥실 움직입니다.
    /// </summary>
    private void FloatPlatform()
    {
        // Sin 함수를 이용해 부드러운 상하 움직임 계산
        float newY = floatStartPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(floatStartPosition.x, newY, floatStartPosition.z);
    }

    /// <summary>
    /// 설정된 웨이포인트를 따라 움직입니다.
    /// </summary>
    private void MovePlatformAlongWaypoints()
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