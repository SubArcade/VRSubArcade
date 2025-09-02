using System.Collections;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    private HingeJoint hinge;
    private Rigidbody rb;
    private Quaternion startRot;

    [Header("설정")]
    public bool springOn = true; // true=스프링 자동복원, false=버튼 리셋
    public float springPower = 30f;
    public float springDamper = 3f;

    [Header("버튼 리셋용")]
    public float resetSpring = 80f;        // 버튼 리셋 때만 잠깐 강하게
    public float resetDamper = 6f;
    public float stabilizeDelay = 0.2f;    // 리셋 후 안정 대기

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hinge = GetComponent<HingeJoint>();
        startRot = transform.localRotation;


        if (hinge != null)
        {
            hinge.useLimits = true;
            var limits = hinge.limits;
            limits.min = 80f; // 뒤
            limits.max = -5f;   // 앞
            hinge.limits = limits;
        }

        if (hinge != null && springOn)
        {
            // 스프링 자동 모드

            JointSpring spring = hinge.spring;
            spring.spring = springPower;
            spring.damper = springDamper;
            spring.targetPosition = 0;
            hinge.spring = spring;
            hinge.useSpring = true;
        }
        else if (hinge != null)
        {
            // 버튼 리셋 모드
            hinge.useSpring = false;
        }
    }

    public void ResetTarget()
    {
        if (!springOn)
        {
            StartCoroutine(ResetRoutine());
        }
    }

    private IEnumerator ResetRoutine()
    {
        // 관성 제거
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 잠깐 강한 스프링으로 0도(세운 상태)로 당김
        var prevSpring = hinge.spring;

        var s = hinge.spring;
        s.spring = resetSpring;
        s.damper = resetDamper;
        s.targetPosition = 0f;
        hinge.spring = s;
        hinge.useSpring = true;

        // 힌지 각도가 거의 0도에 도달할 때까지 대기
        while (Mathf.Abs(hinge.angle) > 1.5f)
            yield return null;

        // 안정 대기
        yield return new WaitForSeconds(stabilizeDelay);

        // 버튼 리셋 모드라면 스프링 원복/해제
        
        if (!springOn) hinge.useSpring = false ;
        hinge.spring = prevSpring;

        // 최종적으로 흔들림 방지
        //rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;

    }
}
