using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    public GameObject hitEffect;
    public float speed = 50f;
    private Rigidbody rb;

    void Start()
    {
        Destroy(gameObject, 1.5f);
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Target"))
        {
            // 충돌 시 이펙트를 생성합니다.
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
