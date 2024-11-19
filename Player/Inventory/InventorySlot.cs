using System;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Stats")]

    public float Weight;

    [Header("Rarity")]
    public ItemRarity Rarity { get; private set; }

    [Header("Type")]

    public ItemList ItemIndeficator { get; private set; }

    public ItemTypes ItemType { get; private set; }

    [Header("Parametrs")]

    public bool Stacked { get; private set; }

    public int ItemCount { get; private set; }

    public AudioClip[] PickupClip { get; private set; }

    [Header("Item")]

    public string ItemName { get; private set; }

    public GameObject Item { get; private set; }

    [Header("UI")]

    [SerializeField] private Texture _standartItemIcon;
    public Texture ItemIcon { get; private set; }

    [SerializeField] private GameObject _selectionFrame;
    public GameObject SelectionFrame { get { return _selectionFrame; } }


    [Header("Other")]

    [SerializeField] private InventorySlotUI _inventorySlotUI;

    public Inventory Inventory;

    public static event Action<InventorySlot> OnItemSlotDestroy;
    public static event Action<BulletType> OnDropAmmoCountsChange;

    private void Start()
    {
        _inventorySlotUI.ItemName.text = ItemName;

        UpdateUIItemCount();

        GetComponent<RawImage>().material.mainTexture = ItemIcon;

    }

    public void UpdateUIItemCount()
    {
        if (Stacked)
        {
            _inventorySlotUI.ItemCount.text = ItemCount.ToString();
        }
    }

    public void Drop()
    {
        GameObject item = Instantiate(Item, DropPoint.ItemDropPosition.position, Item.transform.rotation);

        Item itemComponent = item.GetComponent<Item>();

        itemComponent.ItemCount = 1;

        if (ItemType == ItemTypes.Weapon)
        {
            GunData gunData = GetComponent<GunData>();

            GunData itemGunData = item.AddComponent<GunData>();

            itemGunData.MagazineAmmo = gunData.MagazineAmmo;
        }

        ChangeItemCount(-1);
    }

    public void OnItemClick()
    {
        if (PickupClip.Length > 0)
        {
            int random = UnityEngine.Random.Range(0, PickupClip.Length);

            PickubleAudioSource.AudioSource.PlayOneShot(PickupClip[random]);
        }

        Inventory.ChooseItem(gameObject, this);
    }

    public void ChangeItemCount(int count)
    {
        ItemCount += count;

        if (ItemType == ItemTypes.Ammo)
        {
            for (int i = 0; i < Enum.GetValues(typeof(BulletType)).Length; i++)
            {
                BulletType bulletType = (BulletType)i;

                if (ItemIndeficator.ToString() == bulletType.ToString())
                {
                    OnDropAmmoCountsChange?.Invoke(bulletType);

                    break;
                }
            }
        }

        if (ItemCount <= 0)
        {
            OnItemSlotDestroy?.Invoke(this);

            Destroy(gameObject);
        }

        UpdateUIItemCount();
    }

    /// <summary>
    /// Set Properties To Current Inventory Slot
    /// </summary>
    /// <param name="properties"></param>
    public void SetProperties(ItemSlotProperties properties)
    {
        Rarity = properties.Rarity;
        ItemIndeficator = properties.ItemIndeficator;
        ItemType = properties.ItemType;
        Stacked = properties.Stacked;
        PickupClip = properties.PickupClip;
        Item = properties.Item;
        ItemName = properties.ItemName;

        if (properties.ItemIcon != null)
        {
            ItemIcon = properties.ItemIcon;
        }
        else
            ItemIcon = _standartItemIcon;

    }
}
