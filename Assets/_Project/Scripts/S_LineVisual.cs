using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeVisual : MonoBehaviour
{
    [Header("줄의 시작점 (실린더)")]
    public Transform startPoint;

    [Header("줄의 끝점 (BAG)")]
    public Transform endPoint;

    [Header("중간 처짐 정도")]
    public float sagAmount = 0.2f;

    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 3; // 시작점, 중간점, 끝점
    }

    void Update()
    {
        if (startPoint == null || endPoint == null) return;

        Vector3 p0 = startPoint.position;
        Vector3 p2 = endPoint.position;
        Vector3 mid = (p0 + p2) / 2;

        // 중간 점을 아래로 살짝 처지게
        Vector3 sag = Vector3.down * sagAmount;
        Vector3 p1 = mid + sag;

        line.SetPosition(0, p0);
        line.SetPosition(1, p1);
        line.SetPosition(2, p2);
    }
}
