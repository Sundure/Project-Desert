using System;
using UnityEngine;

public class GunSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] _gunSlot = new GameObject[2];
    [SerializeField] private GameObject _changedGun;

    public static event Action<bool> ChangeFireMod;

    private void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.Alpha1) && _changedGun != _gunSlot[0])
        {
            if (_changedGun != null)
            {
                _changedGun.SetActive(false);
            }

            _changedGun = _gunSlot[0];

            _changedGun.SetActive(true);

            ChangeFireMod?.Invoke(_changedGun.GetComponent<Weapon>().GunStats.Automatic);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && _changedGun != _gunSlot[1])
        {
            if (_changedGun != null)
            {
                _changedGun.SetActive(false);
            }

            _changedGun = _gunSlot[1];

            _changedGun.SetActive(true);

            ChangeFireMod?.Invoke(_changedGun.GetComponent<Weapon>().GunStats.Automatic);
        }
    }
}
