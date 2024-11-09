using TMPro;
using UnityEngine;

public class AmmoIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ammo;
    [SerializeField] private TextMeshProUGUI _magazineAmmo;

    private int _lastValue;
    private BulletType _lastAmmo;

    private void Awake()
    {
        GunSwitch.SwitchGun += SwitchUI;
        Weapon.ChangeAmmoUI += ChangeValue;
        AmmoBox.AmmoPickup += ValueCheck;

        gameObject.SetActive(false);
    }

    private void ChangeValue(int ammo, BulletType ammoIndex)
    {
        _ammo.text = $"{ammo}";

        if (BulletInventory.Ammo[(int)ammoIndex] > 999)
        {
            _magazineAmmo.fontSize = 30;
        }
        else
        {
            _magazineAmmo.fontSize = 40;
        }

        _magazineAmmo.text = $"{BulletInventory.Ammo[(int)ammoIndex]}";

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
        GunSwitch.SwitchGun -= SwitchUI;
        Weapon.ChangeAmmoUI -= ChangeValue;
        AmmoBox.AmmoPickup -= ValueCheck;
    }
}
