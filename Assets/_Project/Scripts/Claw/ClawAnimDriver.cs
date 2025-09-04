using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAnimDriver : MonoBehaviour
{
    public ClawBtn _clawBtn;
    public ClawJoyStick _clawJoySitck;
    [SerializeField] public Animator anim;
    
    [SerializeField] private Transform cablePoint;
    [SerializeField] private Transform cable;
    
    [SerializeField] float pointDownY = -20f;   // 케이블 포인트 최종 y
    [SerializeField] float cableDownPosY = 10f; // 케이블 오브젝트 최종 y
    [SerializeField] float cableDownScaleY = 10f; // 케이블 스케일 y
    
    [SerializeField] float defaultDuration = 3f;
    [SerializeField] AnimationCurve ease = AnimationCurve.EaseInOut(0,0,1,1);

    // 초기 상태 저장
    Vector3 pointInitLocalPos;
    Vector3 cableInitLocalPos;
    Vector3 cableInitLocalScale;

    Coroutine tweenCR;
    
    // public void OnMoveEnd()
    // {
    //     print("OnMoveEnd");
    //     anim.SetBool("isGrab", _clawBtn.isGrabberActive);
    //     _clawBtn.isGrabberActive = true;
    //     _clawJoySitck.SetCombackPos();
    // }
    
    // 집게 잡기
    public void OnGrab()
    {
        //print("OnGrab");
        ClawDown();
    }
    
    // 집게 풀기
    public void OnPlaceMent()
    {
        //print("OnPlaceMent");
        _clawBtn.SetEnabled(false);
    }
    
    void Awake()
    {
        pointInitLocalPos = cablePoint.localPosition;
        cableInitLocalPos = cable.localPosition;
        cableInitLocalScale = cable.localScale;
    }

    public void ClawDown() => StartMove(true,  defaultDuration);
    public void ClawUp() => StartMove(false, defaultDuration);

    void StartMove(bool down, float seconds)
    {
        if (tweenCR != null) StopCoroutine(tweenCR);
        tweenCR = StartCoroutine(MoveRoutine(down, Mathf.Max(0.0001f, seconds)));
    }

    IEnumerator MoveRoutine(bool down, float duration)
    {
        Vector3 p0 = cablePoint.localPosition;
        Vector3 cp0 = cable.localPosition;
        Vector3 s0 = cable.localScale;
        
        Vector3 p1 = p0;  p1.y  = down ? pointDownY     : pointInitLocalPos.y;
        Vector3 cp1 = cp0; cp1.y = down ? cableDownPosY : cableInitLocalPos.y;
        Vector3 s1 = s0;  s1.y  = down ? cableDownScaleY: cableInitLocalScale.y;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float k = ease.Evaluate(t);

            cablePoint.localPosition = Vector3.Lerp(p0,  p1,  k);
            cable.localPosition      = Vector3.Lerp(cp0, cp1, k);
            cable.localScale         = Vector3.Lerp(s0,  s1,  k);

            elapsed += Time.deltaTime;
            yield return null;
        }
        
        cablePoint.localPosition = p1;
        cable.localPosition      = cp1;
        cable.localScale         = s1;

        tweenCR = null;
        
        // 집게 상호작용
        if (down)
        {
           // print("Down");
            anim.SetTrigger("Grab");
            //_clawBtn.SetEnabled(true);
            //ClawUp();
        }
        else
        {
         //   print("Up");
            _clawJoySitck.SetCombackPos();
        }
        
    }
    
    public void OnSetGrabPos()
    {
        print("onSetGrabPos");
        _clawBtn.SetEnabled(true);   
        ClawUp();
    }
}
