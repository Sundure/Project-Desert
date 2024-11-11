using UnityEngine;

public class AmmoBox : MonoBehaviour, IPickuble
{
    public static event System.Action<BulletType> AmmoPickup;

    [SerializeField] private BulletType _bulletType;

    [SerializeField] private int _bulletCount;

    [SerializeField] private AudioClip[] _pickupClip;

    public void Pickup()
    {
        BulletInventory.Ammo[(int)_bulletType] += _bulletCount;

        AmmoPickup?.Invoke(_bulletType);

        int random = Random.Range(0, _pickupClip.Length);

        PickubleAudioSource.AudioSource.PlayOneShot(_pickupClip[random]);

        //Destroy(gameObject);
    }
}
