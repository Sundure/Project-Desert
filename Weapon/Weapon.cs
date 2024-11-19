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

    [SerializeField] private GunData _gunData;

    private void Awake()
    {
        _gunData.GunAwake = false;
        _gunData.Reloading = false;

        _gunData.GunStats.ReloadTime = _reloadAnimClip.length;

        _gunData.GunStats.AwakeTime = _awakeClip.length;
    }

    private void Start()
    {
        ChangeAmmoUI?.Invoke(_gunData.MagazineAmmo, _gunData.GunStats.BulletType);
    }

    private void Update()
    {
        if (Player.CanUseGun)
        {
            if (_gunData.GunStats.Automatic && _gunData.MagazineAmmo > 0)
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
        if (_gunData.Reloading == false && _gunData.MagazineAmmo > 0 && _gunData.CanShoot)
        {
            OnFire?.Invoke();

            FireAnim();

            OnRecoilFire?.Invoke(_gunData.GunStats.Recoil);

            if (Player.Aiming)
            {
                OnRecoilFire?.Invoke(_gunData.GunStats.Recoil / 3);
            }
            else
            {
                OnRecoilFire?.Invoke(_gunData.GunStats.Recoil);
            }

            AudioShoot();

            _gunData.MagazineAmmo--;

            ChangeAmmoUI?.Invoke(_gunData.MagazineAmmo, _gunData.GunStats.BulletType);

            Debug.Log($"Fire {this}");

            int layerMask = ~LayerMask.GetMask("Player");

            Ray ray = PlayerCamera.Camera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                SpawnBullet?.Invoke(_bulletSpawnPoint, hit.point);

                if (hit.collider.gameObject.TryGetComponent(out IFoced iForce))  // IForced Always Must Be First
                {
                    iForce.TakeForce(hit.point, _gunData.GunStats.Damage);
                }

                if (hit.collider.gameObject.TryGetComponent(out IHitable iHit))
                {
                    iHit.TakeDamage(_gunData.GunStats.Damage);
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
        else if (_gunData.Reloading == false && _gunData.CanShoot)
        {
            _audioSource.PlayOneShot(_emptyGunClip);

            StartCoroutine(ShootRate());

            return;
        }
    }

    virtual protected void Reload()
    {
        if (BulletInventory.InventoryAmmoSlot[(int)_gunData.GunStats.BulletType] == null)
        {
            return;
        }

        if (_gunData.MagazineAmmo < _gunData.GunStats.MaxMagazineAmmo && BulletInventory.InventoryAmmoSlot[(int)_gunData.GunStats.BulletType].ItemCount > 0 && _gunData.Reloading == false && _gunData.GunAwake == false)
        {
            if (Player.Aiming)
            {
                Aim();
            }

            _animator.SetTrigger("Reload");

            AudioReload();

            _gunData.CanReload = false;
            _gunData.Reloading = true;

            StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator AwakeGun()
    {
        _gunData.CanShoot = false;

        yield return new WaitForSeconds(_gunData.GunStats.AwakeTime);

        _gunData.CanShoot = true;
        _gunData.GunAwake = false;
        _gunData.CanReload = true;
    }

    private IEnumerator ShootRate()
    {
        _gunData.CanShoot = false;

        yield return new WaitForSeconds(_gunData.GunStats.ShootRate);

        _gunData.CanShoot = true;
    }

    private IEnumerator ReloadCoroutine()
    {
        if (_gunData.Reloading)
        {
            Debug.Log("Reloading");

            yield return new WaitForSeconds(_gunData.GunStats.ReloadTime);

            _gunData.Reloading = false;
            _gunData.CanReload = true;

            _gunData.MagazineAmmo += BulletInventory.TakeAmmo((int)_gunData.GunStats.BulletType, _gunData.GunStats.MaxMagazineAmmo - _gunData.MagazineAmmo);

            ChangeAmmoUI?.Invoke(_gunData.MagazineAmmo, _gunData.GunStats.BulletType);
        }
    }

    virtual protected void FireAnim()
    {
        _animator.SetTrigger("Fire");
    }

    private void Aim()
    {
        if (_gunData.Reloading == false && _gunData.CanReload)
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
        _gunData.Reloading = false;

        _gunData.GunAwake = false;

        Player.ChangeAim -= DisableAim;

        ChangeAim?.Invoke(Player.Aiming);
    }

    virtual protected void OnEnable()
    {
        _animator.SetTrigger("Switch Gun");

        _gunData.GunAwake = true;

        StartCoroutine(AwakeGun());

        ChangeAmmoUI?.Invoke(_gunData.MagazineAmmo, _gunData.GunStats.BulletType);

        Player.Aiming = false;

        Player.ChangeAim += DisableAim;

        ChangeAim?.Invoke(Player.Aiming);
    }
}
