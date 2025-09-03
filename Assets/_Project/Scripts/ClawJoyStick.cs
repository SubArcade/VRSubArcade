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

        Vector3 worldTargetX = railX.parent
            ? railX.parent.TransformPoint(firstPosRailX)
            : firstPosRailX;
        Vector3 worldTargetZ = railZ.parent
            ? railZ.parent.TransformPoint(firstPosRailZ)
            : firstPosRailZ;

        // 두 레일이 동시에 원위치로 갈 때까지
        while ((railX.position - worldTargetX).sqrMagnitude > 0.00001f || (railZ.position - worldTargetZ).sqrMagnitude > 0.00001f)
        {
            railXRb.MovePosition(Vector3.MoveTowards(railX.position, worldTargetX, returnSpeed * Time.fixedDeltaTime));
            railZRb.MovePosition(Vector3.MoveTowards(railZ.position, worldTargetZ, returnSpeed * Time.fixedDeltaTime));

            yield return new WaitForFixedUpdate();
        }
        
        railX.localPosition = firstPosRailX;
        railZ.localPosition = firstPosRailZ;
        print("트리거 실행");
        anim.SetTrigger("PlaceMent");
        isMove = false;
    }

}
