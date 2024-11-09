using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Stats")]

    public float Weight;

    [Header("Rarity")]

    public ItemRarity Rarity;

    [Header("Type")]

    public ItemList ItemType;

    public bool Resource;
    public bool Ammo;
    public bool Weapon;

    [Header("Parametrs")]

    public bool Stacked;

    public int ItemCount = 1;

    public AudioClip[] PickupClip;

    [Header("Icon")]

    public UnityEngine.UI.Image ItemIcon;

    [Header("Item")]

    public GameObject HandItem;

    public GameObject GroundItem;

    public int ContainerCount;

    public void Pickup()
    {
        if (PickupClip.Length > 0)
        {
            int random = Random.Range(0, PickupClip.Length);

            PickubleAudioSource.AudioSource.PlayOneShot(PickupClip[random]);
        }

        Destroy(gameObject);
    }

}
