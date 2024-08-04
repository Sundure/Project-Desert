using UnityEngine;

[CreateAssetMenu(menuName = "Scripts/Weapon/GunData")]
public class GunStats : ScriptableObject
{
    private void Awake()
    {
        Ammo = MaxAmmo;
        GunAmmo = MaxGunAmmo;
    }

    [Header("Stats")]
    public int MaxAmmo;
    public int MaxGunAmmo;
    public int GunAmmo;
    public int Ammo;

    public float Damage;

    public float Accuracy;
    public float ShootRate;

    public float Recoil;

    public bool Automatic;

    [Header("Reloading")]

    public float ReloadTime;

    public bool CanReload;
    public bool Reloading;

    [Header("Gun")]
    public bool CanShoot = true;
    public float AwakeTime;
}
