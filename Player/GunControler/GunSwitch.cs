using System;
using UnityEngine;

public class GunSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] _gunSlot = new GameObject[2];
    [SerializeField] private GameObject _changedGun;

    public static event Action<bool> ChangeFireMod;

    private void Start()
    {
        Car.OnCarSeat += DisableGun;

        for (int i = 0; i < _gunSlot.Length; i++)
        {
            if (_gunSlot[i].activeSelf && _changedGun == null)
            {
                foreach (GameObject gun in _gunSlot)
                {
                    gun.SetActive(false);
                }

                _changedGun = _gunSlot[i];
                _changedGun.SetActive(true);

                ChangeFireMod?.Invoke(_changedGun.GetComponent<Weapon>().GunStats.Automatic);
            }

        }

    }

    private void Update()
    {
        if (Player.CanUseGun)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeGun(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeGun(1);
            }
        }
    }

    private void DisableGun()
    {
        if (_changedGun != null)
        {       
            _changedGun.SetActive(false);

            _changedGun = null;
        }
    }

    private void ChangeGun(int GunSlot)
    {
        if (_changedGun == _gunSlot[GunSlot])
        {
            DisableGun();

            return;
        }

        if (_changedGun != null)
        {
            _changedGun.SetActive(false);
        }

        _changedGun = _gunSlot[GunSlot];

        _changedGun.SetActive(true);

        ChangeFireMod?.Invoke(_changedGun.GetComponent<Weapon>().GunStats.Automatic);
    }

    private void OnDestroy()
    {
        Car.OnCarSeat -= DisableGun;
    }
}
