using UnityEngine;
using System.Collections;

public class ClawJoyStick : MonoBehaviour
{
    [SerializeField] private Rigidbody joystickRb;      // 스틱은 isKinematic=false
    [SerializeField] private Transform joystickBase;    // 스틱 베이스
    [SerializeField] private float maxDeflect = 0.3f;   // 스틱 기울기
    [SerializeField] private float deadZone = 0.08f;
    
    [SerializeField] private Transform railX;
    [SerializeField] private Transform railZ;
    [SerializeField] private Rigidbody railXRb;
    [SerializeField] private Rigidbody railZRb;
    
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float minX = -1.5f;
    [SerializeField] private float maxX =  1.5f;
    [SerializeField] private float minZ = -1.5f;
    [SerializeField] private float maxZ =  1.5f;

    private Vector3 initialWorldCom;
    
    // 처음 포지션
    private Vector3 firstPosRailX;
    private Vector3 firstPosRailZ;
    public bool isMove = false;
    [SerializeField] private float returnSpeed = 2f; // 돌아가는 속도
    public Animator anim;
    public ClawBtn clawBtn;
    
    private IEnumerator Start()
    {
        yield return null;
        initialWorldCom = joystickRb.worldCenterOfMass;
        firstPosRailX = railX.localPosition;
        firstPosRailZ = railZ.localPosition;
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        // ── 1) 스틱 기울기(무게중심 오프셋) → -1..1 입력
        Vector3 worldCom    = joystickRb.worldCenterOfMass;
        Vector3 currentLocal = joystickBase.InverseTransformPoint(worldCom);
        Vector3 initialLocal = joystickBase.InverseTransformPoint(initialWorldCom);
        Vector3 localDelta   = currentLocal - initialLocal;

        float x = Mathf.Clamp(localDelta.x / maxDeflect, -1f, 1f);
        float z = Mathf.Clamp(localDelta.z / maxDeflect, -1f, 1f);
        x = ApplyDeadZone(x, deadZone);
        z = ApplyDeadZone(z, deadZone);

        // ── 2) 다음 로컬 위치 계산(레일별 1축만 변경)
        Vector3 nextLocalX = railX.localPosition + Vector3.right   * (x * moveSpeed * dt);
        Vector3 nextLocalZ = railZ.localPosition + Vector3.forward * (z * moveSpeed * dt);
        nextLocalX.x = Mathf.Clamp(nextLocalX.x, minX, maxX);
        nextLocalZ.z = Mathf.Clamp(nextLocalZ.z, minZ, maxZ);
        
        if(!isMove)
        {
            if (!Mathf.Approximately(x, 0f))
            {
                Vector3 worldTargetX = railX.parent
                    ? railX.parent.TransformPoint(nextLocalX)
                    : nextLocalX;
                railXRb.MovePosition(worldTargetX);
            }

            if (!Mathf.Approximately(z, 0f))
            {
                Vector3 worldTargetZ = railZ.parent
                    ? railZ.parent.TransformPoint(nextLocalZ)
                    : nextLocalZ;
                railZRb.MovePosition(worldTargetZ);
            }
        }
        
        //Debug.DrawRay(joystickBase.position, joystickBase.TransformDirection(new Vector3(x, 0f, z)), Color.cyan, 0f, false);
    }

    private float ApplyDeadZone(float v, float dz)
    {
        float a = Mathf.Abs(v);
        if (a < dz) return 0f;
        return Mathf.Sign(v) * Mathf.Clamp01((a - dz) / (1f - dz));
    }

    public void SetCombackPos()
    {
        if (!isMove)
            StartCoroutine(MoveBackRoutine());
    }

    private IEnumerator MoveBackRoutine()
    {
        isMove = true;

        // 1) 월드 목표 계산 (firstPos*는 "로컬" 저장값이라고 가정)
        Vector3 worldTargetX = (railX.parent != null) ? railX.parent.TransformPoint(firstPosRailX) : firstPosRailX;
        Vector3 worldTargetZ = (railZ.parent != null) ? railZ.parent.TransformPoint(firstPosRailZ) : firstPosRailZ;

        // 2) 스냅/시간 파라미터 (스케일 영향 반영)
        // 평균 스케일(비균등 스케일도 어느정도 커버)
        float scaleMag = (railX.lossyScale.x + railX.lossyScale.y + railX.lossyScale.z) / 3f;
        float snapDist = Mathf.Max(0.002f, 0.01f * scaleMag); // 가까우면만 스냅
        float snapDistSqr = snapDist * snapDist;

        // 동적 타임아웃 = 최대거리 / 속도 + 버퍼
        float distX = Vector3.Distance(railX.position, worldTargetX);
        float distZ = Vector3.Distance(railZ.position, worldTargetZ);
        float maxDist = Mathf.Max(distX, distZ);
        float speed = Mathf.Max(0.0001f, returnSpeed);
        float timeOut = maxDist / speed + 0.25f;          // 필요 시간 + 여유 0.25s
        timeOut = Mathf.Min(timeOut, 6f);                 // 말도 안 되게 오래 끌지 않도록 상한
        float t = 0f;

        // Rigidbody 보간 추천(부드럽게): railXRb.interpolation = RigidbodyInterpolation.Interpolate; (초기화 코드에서)
        while (true)
        {
            float dx2 = (railX.position - worldTargetX).sqrMagnitude;
            float dz2 = (railZ.position - worldTargetZ).sqrMagnitude;

            // 둘 다 충분히 가까우면 종료
            if (dx2 <= snapDistSqr && dz2 <= snapDistSqr) break;
            // 시간이 초과되면 "스냅 없이" 종료 (멀리서 텔레포트 방지)
            if (t >= timeOut) break;

            float step = speed * Time.fixedDeltaTime;
            railXRb.MovePosition(Vector3.MoveTowards(railX.position, worldTargetX, step));
            railZRb.MovePosition(Vector3.MoveTowards(railZ.position, worldTargetZ, step));

            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime;
        }
        
        if ((railX.position - worldTargetX).sqrMagnitude <= snapDistSqr)
            railXRb.MovePosition(worldTargetX);
        if ((railZ.position - worldTargetZ).sqrMagnitude <= snapDistSqr)
            railZRb.MovePosition(worldTargetZ);
        
        railXRb.velocity = Vector3.zero;
        railZRb.velocity = Vector3.zero;
        railXRb.angularVelocity = Vector3.zero;
        railZRb.angularVelocity = Vector3.zero;
        
        // railX.localPosition = firstPosRailX;
        // railZ.localPosition = firstPosRailZ;

        anim.SetTrigger("Placement");
        isMove = false;
        clawBtn.isGrabberActive = true;
    }
}
