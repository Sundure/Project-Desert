using System;
using UnityEngine;

public class GunControler : MonoBehaviour
{
    public static event Action Fire;
    public static event Action Reload;
    public static event Action Aiming;

    private bool _automatic;

    private void Start()
    {
        GunSwitch.ChangeFireMod += ChangeFireMode;
    }

    private void Update()
    {
        if (Player.CanUseGun)
        {
            if (_automatic)
            {
                if (Input.GetButton("Fire1"))
                {
                    Fire?.Invoke();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Fire?.Invoke();
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload?.Invoke();
            }

            if (Input.GetButtonDown("Fire2"))
            {
                Aiming?.Invoke();
            }
        }
    }

    private void ChangeFireMode(bool gunMode)
    {
        _automatic = gunMode;
    }

    private void OnDestroy()
    {
        GunSwitch.ChangeFireMod -= ChangeFireMode;
    }
}
