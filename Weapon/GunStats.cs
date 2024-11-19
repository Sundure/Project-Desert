using UnityEngine;

[CreateAssetMenu(menuName = "Scripts/Weapon/GunData")]
public class GunStats : ScriptableObject
{

    [Header("Stats")]
    public int MaxMagazineAmmo;

    public float Damage;

    public float Accuracy;
    public float ShootRate;

    public float Recoil;

    public bool Automatic;

    [Header("Reloading")]

    public float ReloadTime;

    [Header("Gun")]
    public float AwakeTime;

    public GunType GunType;
    public BulletType BulletType;
}
