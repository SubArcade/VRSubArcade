using UnityEngine;

public class HittableTarget : MonoBehaviour
{
    [Header("점수 설정")]
    public int scoreValue = 10; // 획득할 점수

    [Header("연결할 오브젝트")]
    public PlatformMover platformMover; // 움직임을 제어하는 PlatformMover 스크립트

    private Rigidbody rb;
    private bool wasHit = false; // 중복 점수 획득 방지용 플래그

    // --- 리셋을 위한 초기 상태 저장용 변수들 ---
    private RigidbodyConstraints initialConstraints;
    private bool initialUseGravity;
    private bool initialIsKinematic;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 리셋을 위해 타겟의 초기 상태를 모두 저장합니다.
        SaveInitialState();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 총알에 맞았고, 아직 점수 획득 전이라면
        if (!wasHit && collision.gameObject.CompareTag("Bullet"))
        {
            wasHit = true; // 점수는 한 번만 획득하도록 처리

            // 1. 점수 획득
            ShootingManager.Instance.AddScore(scoreValue);

            // 2. PlatformMover 비활성화
            if (platformMover != null)
            {
                platformMover.enabled = false;
            }

            // 3. Rigidbody 상태 변경하여 땅에 떨어지도록 함
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.None; // 모든 물리 제한 해제
                rb.useGravity = true;  // 중력 활성화
                rb.isKinematic = false; // Kinematic 비활성화 (중력 적용을 위해)
            }
        }
    }

    /// <summary>
    /// 타겟을 처음 상태로 되돌립니다.
    /// </summary>
    public void ResetTarget()
    {
        // Rigidbody 상태를 저장해둔 초기 상태로 되돌립니다.
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = initialIsKinematic;
            rb.useGravity = initialUseGravity;
            rb.constraints = initialConstraints;
        }

        // 위치는 부모 기준 원점, 회전은 (0, 90, 0)으로 되돌립니다.
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 90, 0);

        // PlatformMover가 있었다면 다시 활성화합니다.
        if (platformMover != null)
        {
            platformMover.enabled = true;
        }

        // 다시 점수를 획득할 수 있도록 플래그를 리셋합니다.
        wasHit = false;
    }

    /// <summary>
    /// 리셋을 위해 현재 상태를 변수에 저장하는 함수
    /// </summary>
    private void SaveInitialState()
    {
        if (rb != null)
        {
            initialConstraints = rb.constraints;
            initialUseGravity = rb.useGravity;
            initialIsKinematic = rb.isKinematic;
        }
    }
}