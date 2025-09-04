using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
 
public class ClawBtn : MonoBehaviour
{
    [SerializeField] private Animator anim; // 크루 머신 집게 애니메이션Z
    public bool isGrabberActive = true;

    [SerializeField] private ClawJoyStick _clawJoySitck;
    [SerializeField] private ClawAnimDriver _clawAnimDriver;

    [SerializeField] private GameObject arm1;
    [SerializeField] private GameObject arm2;
    [SerializeField] private GameObject arm3;
    
    [SerializeField] private GameObject arm1Rig;
    [SerializeField] private GameObject arm2Rig;
    [SerializeField] private GameObject arm3Rig;
    
    private Vector3 _rotate1 = new Vector3(30.699f, -3.279f, 0.17f);
    private Vector3 _rotate2 = new Vector3(-15.37f, -1.996f, 28.586f);
    private Vector3 _rotate3 = new Vector3(-14.411f, 6.309f, -29.381f);
    public void Press()
    {
        //print("press");
        if (anim && isGrabberActive)
        {
            _clawAnimDriver.OnGrab();
            isGrabberActive = false;
        }
    }

    public void SetEnabled(bool on)
    {
        arm1.GetComponent<RotationConstraint>().enabled = on;
        arm2.GetComponent<RotationConstraint>().enabled = on;
        arm3.GetComponent<RotationConstraint>().enabled = on;
        
        if (on)
        {
            StartCoroutine(ApplyRotationDelayed(0.3f));
        }
    }

    private IEnumerator ApplyRotationDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        arm1Rig.transform.localRotation = Quaternion.Euler(_rotate1);
        arm2Rig.transform.localRotation = Quaternion.Euler(_rotate2);
        arm3Rig.transform.localRotation = Quaternion.Euler(_rotate3);
    }
}
 