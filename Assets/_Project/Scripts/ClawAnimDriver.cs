using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAnimDriver : MonoBehaviour
{
    public ClawBtn _clawBtn;
    public ClawJoyStick _clawJoySitck;
    [SerializeField] public Animator anim;
    
    public void OnMoveEnd()
    {
        print("OnMoveEnd");
        anim.SetBool("isActive", _clawBtn.isGrabberActive);
        _clawBtn.isGrabberActive = true;
        _clawJoySitck.SetCombackPos();
    }
}
