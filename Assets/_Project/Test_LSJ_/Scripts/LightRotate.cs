using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotate : MonoBehaviour
{
    private Quaternion initialRotation;
    public float rotateSpeed = 100f;
    
    private void Start()
    {
        initialRotation = transform.rotation;
        StartCoroutine(RotateLight());
    }

    IEnumerator RotateLight()
    {
        float x = this.transform.rotation.eulerAngles.x;
        while (true)
        {
            x += Time.deltaTime * rotateSpeed;
            transform.rotation = Quaternion.Euler(x, initialRotation.eulerAngles.y, initialRotation.eulerAngles.z);
            yield return null;
        }
    }
}
