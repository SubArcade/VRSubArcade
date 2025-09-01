using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private int count = 0;
    private bool trigger1Passed = false;
    private bool trigger2Passed = false;
    private BasketballManager manager;


    private void OnEnable()
    {
        count = 0;
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
        else if (other.CompareTag("Ground"))
        {
            count = 0;
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
