using System;
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public static event Action<float> OnRecoilFire;
    public static event Action OnFire;
    public static event Action<Transform> SpawnBullet;
    public static event Action<Vector3, Rigidbody> OnHit;

    [Header("Fire Point")]
    [SerializeField] protected Transform _raycastPoint;

    [Header("Audio")]
    [SerializeField] protected AudioSource _audioSource;

    [SerializeField] private AudioClip _shootClip;
    [SerializeField] private AudioClip _reloadClip;
    [SerializeField] private AudioClip _emptyGunClip;

    [Header("Data")]
    public GunStats GunStats;

    [Header("Anim")]
    [SerializeField] protected Animator _animator;

    [SerializeField] private AnimationClip _reloadAnimClip;
    [SerializeField] protected AnimationClip _fireAnimClip;
    [SerializeField] protected AnimationClip _awakeClip;


    private void Awake()
    {
        GunStats.Reloading = false;
        GunStats.CanReload = true;
        GunStats.CanShoot = true;

        GunStats.Ammo = GunStats.MaxAmmo;

        GunStats.ReloadTime = _reloadAnimClip.length;

        GunStats.AwakeTime = _awakeClip.length;
    }

    virtual protected void Fire()
    {
        if (GunStats.Reloading == false && GunStats.MagazineAmmo > 0 && GunStats.CanShoot)
        {
            OnFire?.Invoke();
            SpawnBullet?.Invoke(_raycastPoint.transform);

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

            Debug.Log($"Fire {this}");

            Vector3 vector3 = _raycastPoint.forward;

            int layerMask = ~LayerMask.GetMask("Player");

            if (Physics.Raycast(_raycastPoint.position, vector3, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                OnHit?.Invoke(hit.point, hit.rigidbody);

                if (hit.collider.gameObject.TryGetComponent(out IHitable iHit))
                {
                    iHit.TakeDamage(GunStats.Damage);
                }

                Debug.DrawRay(_raycastPoint.position, vector3, Color.green, 10f);

                Debug.Log(hit.collider.name);

            }
            else
            {
                Debug.DrawRay(_raycastPoint.position, vector3, Color.red, 10f);
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
        if (GunStats.MagazineAmmo < GunStats.MaxMagazineAmmo && GunStats.Ammo > 0 && GunStats.Reloading == false)
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

            if (GunStats.Ammo >= GunStats.MaxMagazineAmmo)
            {
                GunStats.Ammo -= GunStats.MaxMagazineAmmo - GunStats.MagazineAmmo;

                GunStats.MagazineAmmo = GunStats.MaxMagazineAmmo;
            }
            else
            {
                GunStats.MagazineAmmo = GunStats.Ammo;

                GunStats.Ammo = 0;
            }

        }
    }

    virtual protected void FireAnim()
    {
        _animator.SetTrigger("Fire");
    }

    virtual protected void Aim()
    {
        Player.Aiming = !Player.Aiming;

        _animator.SetBool("Aim", Player.Aiming);
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
        GunControler.Reload -= Reload;
        GunControler.Fire -= Fire;
        GunControler.Aiming -= Aim;
    }

    virtual protected void OnDisable()
    {
        GunStats.Reloading = false;

        GunControler.Reload -= Reload;
        GunControler.Fire -= Fire;
        GunControler.Aiming -= Aim;
    }

    virtual protected void OnEnable()
    {
        _animator.SetTrigger("Switch Gun");

        StartCoroutine(AwakeGun());

        GunControler.Reload += Reload;
        GunControler.Fire += Fire;
        GunControler.Aiming += Aim;

        Player.Aiming = false;
    }
}
