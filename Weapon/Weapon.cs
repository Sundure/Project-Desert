using System;
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public static event Action<float> OnRecoilFire;
    public static event Action OnFire;

    [Header("Fire Point")]
    [SerializeField] private Transform _raycastPoint;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _shootClip;
    [SerializeField] private AudioClip _reloadClip;
    [SerializeField] private AudioClip _emptyGunClip;

    [Header("Data")]
    public GunStats GunStats;

    [Header("Anim")]
    [SerializeField] protected Animator _animator;

    [SerializeField] private AnimationClip _reloadAnimClip;
    [SerializeField] protected AnimationClip _fireAnimClip;

    private void Awake()
    {
        GunStats.Reloading = false;
        GunStats.CanReload = true;
        GunStats.CanShoot = true;

        GunStats.Ammo = GunStats.MaxAmmo;

        GunStats.ReloadTime = _reloadAnimClip.length;
    }

    virtual protected void Fire()
    {
        if (GunStats.Reloading == false && GunStats.GunAmmo > 0 && GunStats.CanShoot)
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

            GunStats.GunAmmo--;

            Debug.Log("Fire");

            Vector3 vector3 = _raycastPoint.forward;

            int layerMask = ~LayerMask.GetMask("Player");

            if (Physics.Raycast(_raycastPoint.position, vector3, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
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
        }
    }

    virtual protected void Reload()
    {
        if (GunStats.GunAmmo < GunStats.MaxGunAmmo && GunStats.Ammo > 0 && GunStats.Reloading == false)
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

            if (GunStats.Ammo >= GunStats.MaxGunAmmo)
            {
                GunStats.Ammo -= GunStats.MaxGunAmmo;

                GunStats.GunAmmo = GunStats.MaxGunAmmo;
            }
            else
            {
                GunStats.GunAmmo = GunStats.Ammo;

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
        GunControler.Reload -= Reload;
        GunControler.Fire -= Fire;
        GunControler.Aiming -= Aim;
    }

    virtual protected void OnEnable()
    {
        GunControler.Reload += Reload;
        GunControler.Fire += Fire;
        GunControler.Aiming += Aim;

        Player.Aiming = false;
    }
}
