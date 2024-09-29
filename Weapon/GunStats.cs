using UnityEngine;

[CreateAssetMenu(menuName = "Scripts/Weapon/GunData")]
public class GunStats : ScriptableObject
{
    private void Awake()
    {
        MagazineAmmo = MaxMagazineAmmo;
    }

    [Header("Stats")]
    public int MaxMagazineAmmo;
    public int MagazineAmmo;

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

    public GunType GunType;
    public BulletType BulletType;
}
