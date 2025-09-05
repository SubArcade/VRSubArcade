using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class ControlSwap : MonoBehaviour
{
    public InputModeManager InputModeManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            InputModeManager.SwitchMode(InputModeManager.ControlMode.Control_01);
        }
    }

    public void ChangeGun()
    {
        InputModeManager.SwitchMode(InputModeManager.ControlMode.Control_02);
    }
}
