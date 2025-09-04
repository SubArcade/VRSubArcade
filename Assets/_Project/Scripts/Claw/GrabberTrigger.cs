using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabberTrigger : MonoBehaviour
{
    public ClawAnimDriver _ClawAnimDriver;    
    
    void OnTriggerEnter(Collider other)
    {
        _ClawAnimDriver.OnGrab();
    }
}
