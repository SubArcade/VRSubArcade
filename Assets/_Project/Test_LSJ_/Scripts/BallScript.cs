using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private int count = 0;
    private int bounceCount = 0;
    private bool needToPlayBounceSound = false;
    private bool trigger1Passed = false;
    private bool trigger2Passed = false;
    private BasketballManager manager;
    public AudioClip bounce;
    public AudioSource audioSource;
    private bool isDistroyed = false;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        manager = GameObject.FindGameObjectWithTag("BasketballManager").GetComponent<BasketballManager>();
        count = 0;
        bounceCount = 0;
        needToPlayBounceSound = true;
    }

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        count = 0;
        bounceCount = 0;
        needToPlayBounceSound = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BasketballTrigger"))
        {
            if (trigger1Passed == false)
            {
                trigger1Passed = true;
                count++;
                if (count == 2)
                {
                    Goal();
                }
            }

        }
        else if (other.CompareTag("BasketballTrigger2"))
        {
            if (trigger2Passed == false)
            {
                trigger2Passed = true;
                count++;
                if (count == 2)
                {
                    Goal();
                }
            }

        }
        else if (other.CompareTag("BallEndTrigger"))
        {
            audioSource.volume = 1f;
            bounceCount = 0;
            count = 0;
            //Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        audioSource.volume = 1 - (0.2f * bounceCount);
        audioSource.PlayOneShot(bounce);
        bounceCount++;
        if (other.gameObject.CompareTag("Ground"))
        {
            if (isDistroyed == false)
            {
                isDistroyed = true;
                count = 0;
                manager.SpawnBall();
                Destroy(gameObject);
            }
        }
    }

    private void Goal()
    {
        manager.ScoreUpdate();
    }

    public void SetObject(BasketballManager manager)
    {
        this.manager = manager;
    }
}