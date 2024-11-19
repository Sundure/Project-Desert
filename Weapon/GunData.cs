using UnityEngine;

public class GunData : MonoBehaviour
{
    [SerializeField] private GunStats _gunData;

    public GunStats GunStats { get { return _gunData; } }

    [Header("Ammo")]
    public int MagazineAmmo;

    [Header("Reload")]
    public bool CanReload;
    public bool Reloading;

    [Header("Other")]
    public bool CanShoot;

    public bool GunAwake;
}
