using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public Animation anim;
    //public ParticleSystem muzzle;

    public float rpm; // 분당 발사 횟수
    private float interval; //발사 간 시간간격;

    private void Awake()
    {
        if (false == anim) anim = GetComponent<Animation>();
        //if (false == muzzle) muzzle = GetComponent<ParticleSystem>();

        float rps = rpm / 60; //초당 발사 횟수
        interval = 1 / rps;
    }


    float fireTime; // 직전에 발사가 호출된 시간
    bool isTriggerOn;
    private void Update()
    {
        if (!isTriggerOn) return;
        if (Time.time > fireTime + interval) return; //쿨 안돌았으면 리턴
        fireTime = Time.time;


        Fire();
    }

    private void Fire()
    {
    }

    public void PullTrigger(bool isOn)
    {
        isTriggerOn = isOn;

        if (isOn) { 
            anim.Play(); 
            //muzzle.Play();
        } //총알 발사
        else { 
            anim.Stop(); 
            //muzzle.Stop();
        }
    }

}
