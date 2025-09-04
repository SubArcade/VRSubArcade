using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    public AudioSource source;
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
            // 충돌 시 이펙트를 생성.
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            // 이펙트 사운드를 재생합니다.
            if (source != null && source.clip != null)
            {
                // 기존 AudioSource의 볼륨 값을 사용해 재생.
                AudioSource.PlayClipAtPoint(source.clip, transform.position, source.volume);
            }

            // 총알 즉시 파괴합니다.
            Destroy(gameObject);
        }
    }
}
