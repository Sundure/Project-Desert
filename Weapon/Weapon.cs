using System;
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public static event Action<float> OnRecoilFire;
    public static event Action OnFire;
    public static event Action<Transform, Vector3> SpawnBullet;
    public static event Action<bool> ChangeAim;

    public static event Action<int, BulletType> ChangeAmmoUI;

    [Header("Fire Point")]
    [SerializeField] protected Transform _raycastPoint;
    [SerializeField] protected Transform _bulletSpawnPoint;

    [Header("Audio")]
    [SerializeField] protected AudioSource _audioSource;

    [SerializeField] private AudioClip _shootClip;
    [SerializeField] private AudioClip _reloadClip;
    [SerializeField] private AudioClip _emptyGunClip;

    [Header("Anim")]
    [SerializeField] protected Animator _animator;

    [SerializeField] private AnimationClip _reloadAnimClip;
    [SerializeField] private AnimationClip _fireAnimClip;
    [SerializeField] private AnimationClip _awakeClip;

    [Header("Data")]
    [SerializeField] private GunStats GunStats;

    private void Awake()
    {
        GunStats.GunAwake = false;
        GunStats.Reloading = false;
        GunStats.CanShoot = true;

        GunStats.ReloadTime = _reloadAnimClip.length;

        GunStats.AwakeTime = _awakeClip.length;
    }

    private void Start()
    {
        ChangeAmmoUI?.Invoke(GunStats.MagazineAmmo, GunStats.BulletType);
    }

    private void Update()
    {
        if (Player.CanUseGun)
        {
            if (GunStats.Automatic && GunStats.MagazineAmmo > 0)
            {
                if (Input.GetButton("Fire1"))
                {
                    Fire();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Fire();
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            if (Input.GetButtonDown("Fire2"))
            {
                Aim();
            }
        }
    }

    virtual protected void Fire()
    {
        if (GunStats.Reloading == false && GunStats.MagazineAmmo > 0 && GunStats.CanShoot)
        {
            OnFire?.Invoke();

            FireAnim();

            OnRecoilFire?.Invoke(GunStats.Recoil);

            if (Player.Aiming)
            {
                OnRecoilFire.Invoke(GunStats.Recoil / 3);
            }
            else
            {
                OnRecoilFire.Invoke(GunStats.Recoil);
            }

            AudioShoot();

            GunStats.MagazineAmmo--;

            ChangeAmmoUI?.Invoke(GunStats.MagazineAmmo, GunStats.BulletType);

            Debug.Log($"Fire {this}");

            int layerMask = ~LayerMask.GetMask("Player");

            Ray ray = PlayerCamera.Camera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                SpawnBullet?.Invoke(_bulletSpawnPoint, hit.point);

                if (hit.collider.gameObject.TryGetComponent(out IFoced iForce))  // IForced Always Must Be First
                {
                    iForce.TakeForce(hit.point, GunStats.Damage);
                }

                if (hit.collider.gameObject.TryGetComponent(out IHitable iHit))
                {
                    iHit.TakeDamage(GunStats.Damage);
                }


                Debug.DrawRay(ray.origin, ray.direction, Color.green, 10f);

                Debug.Log(hit.collider.name);

            }
            else
            {
                SpawnBullet?.Invoke(_bulletSpawnPoint, ray.GetPoint(50));

                Debug.DrawRay(ray.origin, ray.direction, Color.red, 10f);
            }

            StartCoroutine(ShootRate());
        }
        else if (GunStats.Reloading == false && GunStats.CanShoot)
        {
            _audioSource.PlayOneShot(_emptyGunClip);

            StartCoroutine(ShootRate());

            return;
        }
    }

    virtual protected void Reload()
    {
        if (BulletInventory.InventoryAmmoSlot[(int)GunStats.BulletType] == null)
        {
            return;
        }

        if (GunStats.MagazineAmmo < GunStats.MaxMagazineAmmo && BulletInventory.InventoryAmmoSlot[(int)GunStats.BulletType].ItemCount > 0 && GunStats.Reloading == false && GunStats.GunAwake == false)
        {
            if (Player.Aiming)
            {
                Aim();
            }

            _animator.SetTrigger("Reload");

            AudioReload();

            GunStats.CanReload = false;
            GunStats.Reloading = true;

            StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator AwakeGun()
    {
        GunStats.CanShoot = false;

        yield return new WaitForSeconds(GunStats.AwakeTime);

        GunStats.CanShoot = true;

        GunStats.GunAwake = false;

        GunStats.CanReload = true;
    }

    private IEnumerator ShootRate()
    {
        GunStats.CanShoot = false;

        yield return new WaitForSeconds(GunStats.ShootRate);

        GunStats.CanShoot = true;
    }

    private IEnumerator ReloadCoroutine()
    {
        if (GunStats.Reloading)
        {
            Debug.Log("Reloading");

            yield return new WaitForSeconds(GunStats.ReloadTime);

            GunStats.Reloading = false;
            GunStats.CanReload = true;

            GunStats.MagazineAmmo += BulletInventory.TakeAmmo((int)GunStats.BulletType, GunStats.MaxMagazineAmmo - GunStats.MagazineAmmo);

            ChangeAmmoUI?.Invoke(GunStats.MagazineAmmo, GunStats.BulletType);
        }
    }

    virtual protected void FireAnim()
    {
        _animator.SetTrigger("Fire");
    }

    private void Aim()
    {
        if (GunStats.Reloading == false && GunStats.CanReload && GunStats.CanAim)
        {
            _animator.ResetTrigger("Disable Aim");

            Player.Aiming = !Player.Aiming;

            ChangeAim?.Invoke(Player.Aiming);

            if (Player.Aiming == true)
            {
                _animator.SetTrigger("Enable Aim");
            }
            else
            {
                _animator.SetTrigger("Disable Aim");
            }
        }
    }

    private void DisableAim()
    {
        Player.Aiming = false;

        ChangeAim?.Invoke(Player.Aiming);

        _animator.SetTrigger("Disable Aim");
    }

    virtual protected void AudioShoot()
    {
        _audioSource.PlayOneShot(_shootClip);
    }

    virtual protected void AudioReload()
    {
        _audioSource.PlayOneShot(_reloadClip);
    }

    virtual protected void OnDestroy()
    {
        Player.ChangeAim -= DisableAim;
    }

    virtual protected void OnDisable()
    {
        GunStats.Reloading = false;

        GunStats.GunAwake = false;

        Player.ChangeAim -= DisableAim;

        ChangeAim?.Invoke(Player.Aiming);
    }

    virtual protected void OnEnable()
    {
        _animator.SetTrigger("Switch Gun");

        GunStats.GunAwake = true;

        StartCoroutine(AwakeGun());

        ChangeAmmoUI?.Invoke(GunStats.MagazineAmmo, GunStats.BulletType);

        Player.Aiming = false;

        Player.ChangeAim += DisableAim;

        ChangeAim?.Invoke(Player.Aiming);
    }
}
