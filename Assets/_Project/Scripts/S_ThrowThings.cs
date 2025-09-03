using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThrowThings : MonoBehaviour
{
    [Header("데미지 설정")]
    public int damage = 0;

    [Header("충돌 파티클 프리팹")]
    public GameObject hitParticlePrefab;

    [Header("던질 때 사운드 클립")]
    public AudioClip throwSound;

    [Header("던질 때 사운드 속도 기준")]
    public float throwSoundTriggerSpeed = 2.5f;

    [Header("충돌 사운드 클립")]
    public AudioClip hitSound;

    private bool hasHit = false;
    private bool hasThrownSoundPlayed = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 던지는 속도가 일정 이상일 때 사운드 재생
        if (!hasThrownSoundPlayed && rb != null && rb.velocity.magnitude >= throwSoundTriggerSpeed)
        {
            hasThrownSoundPlayed = true;

            if (throwSound != null)
            {
                AudioSource.PlayClipAtPoint(throwSound, transform.position);
            }
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;

        // 충돌 지점 계산
        ContactPoint contact = collision.contacts[0];
        Vector3 hitPoint = contact.point;
        Quaternion hitRotation = Quaternion.LookRotation(contact.normal);

        if (collision.gameObject.TryGetComponent(out BagHealth bag))
        {
            hasHit = true;
            bag.TakeDamage(damage);
         
            // 파티클 생성
            if (hitParticlePrefab != null)
            {
                GameObject spawnedParticle = Instantiate(hitParticlePrefab, hitPoint, hitRotation);
                Destroy(spawnedParticle, 0.3f);
            }
            // 사운드 재생
            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, hitPoint);
            }

            if (Application.isPlaying)
            {
                Destroy(gameObject);
            }   
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            hasHit = true;

            // 파티클 생성
            if (hitParticlePrefab != null)
            {
                GameObject spawnedParticle = Instantiate(hitParticlePrefab, hitPoint, hitRotation);
                Destroy(spawnedParticle, 0.3f);
            }

            // 사운드 재생
            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, hitPoint);
            }

            if (Application.isPlaying)
            {
                Destroy(gameObject);
            }
        }

    }
}

