using TMPro;
using UnityEngine;

public class AmmoIndicator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _magazineAmmo;
    [SerializeField] private TextMeshProUGUI _inventoryAmmo;

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
        _magazineAmmo.text = $"{ammo}";

        ScaleTextSize((int)ammoIndex, ammo);

        _inventoryAmmo.text = $"{BulletInventory.Ammo[(int)ammoIndex]}";

        _lastValue = ammo;
        _lastAmmo = ammoIndex;
    }

    private void ScaleTextSize(int ammoIndex ,int ammo)
    {
        if (BulletInventory.Ammo[ammoIndex] > 999)
        {
            _inventoryAmmo.fontSize = 30;
        }
        else
        {
            _inventoryAmmo.fontSize = 40;
        }

        if (ammo > 999)
        {
            _magazineAmmo.fontSize = 40;
        }
        else
        {
            _magazineAmmo.fontSize = 50;
        }
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
