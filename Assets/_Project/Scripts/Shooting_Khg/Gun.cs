using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab; //프리팹
    public Transform muzzlePoint; //총알 위치
    public float bulletSpeed = 50f;
    public Animation anim; 
       
    public float interval; //발사 간 시간간격;

    private void Awake()
    {
        if (false == anim) anim = GetComponent<Animation>();
    }


    float fireTime; // 직전에 발사가 호출된 시간

    private void Fire()
    {
        anim.Play();
       
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        // 생성된 총알 스크립트에 속도 값을 넘겨줍니다.
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.speed = bulletSpeed;
        }
    }

    public void PullTrigger(bool isOn)
    {
        // "press" 이벤트(isOn이 true일 때)에만 발사합니다.
        if (isOn)
        {
            // 쿨타임이 지났는지 확인합니다.
            if (Time.time >= fireTime + interval)
            {
                // 마지막 발사 시간을 갱신하고 Fire 메서드를 호출합니다.
                fireTime = Time.time;
                Fire();
            }
        }
        else { 
            anim.Stop();
        }
    }

}
