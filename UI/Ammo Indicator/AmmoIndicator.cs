using TMPro;
using UnityEngine;

public class AmmoIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ammoValue;

    private int _lastValue;
    private BulletType _lastAmmo;

    private void Awake()
    {
        GunSwitch.SwitchGun += SwitchUI;
        Weapon.ChangeAmmoUI += ChangeValue;
        AmmoBox.AmmoPickup += ValueCheck;
    }

    private void ChangeValue(int ammo, BulletType ammoIndex)
    {
        Debug.Log(ammoIndex);

        _ammoValue.text = $"{ammo}/{Inventory.Ammo[(int)ammoIndex]}";

        _lastValue = ammo;
        _lastAmmo = ammoIndex;
    }

    private void SwitchUI(bool enable)
    {
        gameObject.SetActive(enable);
    }

    private void ValueCheck(BulletType bulletType)
    {
        if (bulletType == _lastAmmo)
        {
            ChangeValue(_lastValue, bulletType);
        }
    }

    private void OnDestroy()
    {
        GunSwitch.SwitchGun += SwitchUI;
        Weapon.ChangeAmmoUI -= ChangeValue;
        AmmoBox.AmmoPickup -= ValueCheck;
    }
}
