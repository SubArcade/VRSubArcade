using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
 
public class ClawBtn : MonoBehaviour
{
    [SerializeField] private Animator anim; // 크루 머신 집게 애니메이션Z
    public bool isGrabberActive = true;
    
    public void OnHoverEnter(HoverEnterEventArgs args) => Press();
    public void OnHoverExit(HoverExitEventArgs args) => Release();

    [SerializeField] private ClawJoyStick _clawJoySitck;
    
    public void Press()
    {
        print("press");
        if (anim && isGrabberActive)
        {
            anim.SetBool("isActive", isGrabberActive);
            isGrabberActive = false;
        }
    }

    public void Release()
    {
        print("release");
    }
}
