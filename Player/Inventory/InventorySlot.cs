using System;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("Stats")]

    public float Weight;

    [Header("Rarity")]

    public ItemRarity Rarity;

    [Header("Type")]

    public ItemList ItemIndeficator;

    public ItemTypes ItemType;

    [Header("Parametrs")]

    public bool Stacked;

    public int ItemCount { get; private set; }

    public AudioClip[] PickupClip;

    [Header("Item")]

    public string ItemName;

    public GameObject Item;

    public int SlotNumber;

    [Header("UI")]

    public Texture ItemIcon;

    public GameObject SelectionFrame;

    [Header("Other")]

    [SerializeField] private InventorySlotUI _inventorySlotUI;

    [SerializeField] private GameObject _panel;

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

    public void SwitchPanel()
    {
        _panel.SetActive(!_panel.activeSelf);
    }

    public void Drop()
    {
        GameObject item = Instantiate(Item, DropPoint.ItemDropPosition.position, Quaternion.identity);

        Item itemComponent = item.GetComponent<Item>();

        itemComponent.ItemCount = 1;

        ChangeItemCount(-1);
    }

    public void OnItemClick()
    {
        if (PickupClip.Length > 0)
        {
            int random = UnityEngine.Random.Range(0, PickupClip.Length);

            PickubleAudioSource.AudioSource.PlayOneShot(PickupClip[random]);
        }

        Inventory.ChoseItem(gameObject, this);
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
}
