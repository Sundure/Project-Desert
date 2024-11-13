using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject _inventorySlotPrefab;

    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private GameObject _itemSlot;

    [SerializeField] private GameObject _itemDirectory;

    private GameObject _chousedItem;

    private readonly List<InventorySlot> _itemList = new();

    public static event Action<InventorySlot> ChouseItem;
    public static event Action UnchouseItem;
    public static event Action<BulletType> OnAmmoPickup;

    private void Start()
    {
        _inventoryUI.SetActive(false);

        InventorySlot.OnItemSlotDestroy += OnItemSlotDestroy;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UseInventory();
        }
    }

    private void UseInventory()
    {
        Player.UseInventory = !Player.UseInventory;

        if (Player.UseInventory == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        _inventoryUI.SetActive(Player.UseInventory);

        Player.AutomaticChangeGunEmbark();

        Player.CanInteract = !Player.UseInventory;
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            InventorySlot findedItem = _itemList[i];

            if (findedItem.ItemIndeficator == item.ItemInfo.ItemIndeficator && findedItem.Stacked == true)
            {
                findedItem.ChangeItemCount(item.ItemCount);
                findedItem.Weight += item.ItemInfo.Weight;

                findedItem.UpdateUIItemCount();

                if (item.ItemInfo.ItemType == ItemTypes.Ammo)
                {
                    for (int j = 0; i < Enum.GetValues(typeof(BulletType)).Length; j++)
                    {
                        BulletType findedBulletType = (BulletType)j;

                        if (item.ItemInfo.ItemIndeficator.ToString() == findedBulletType.ToString())
                        {
                            OnAmmoPickup?.Invoke(findedBulletType);
                            break;
                        }
                    }
                }

                return;
            }
        }

        GameObject newSlot = Instantiate(_inventorySlotPrefab, Vector3.zero, Quaternion.identity, _itemSlot.transform);


        ItemSlotProperties properties = ScriptableObject.CreateInstance<ItemSlotProperties>();

        properties = SetProperties(properties, item);


        InventorySlot inventorySlot = newSlot.GetComponent<InventorySlot>();

        inventorySlot.ChangeItemCount(item.ItemCount);
        inventorySlot.Inventory = this;

        inventorySlot.SetProperties(properties);

        Destroy(properties);


        if (item.ItemInfo.ItemType == ItemTypes.Ammo)
        {
            for (int i = 0; i < Enum.GetValues(typeof(BulletType)).Length; i++)
            {
                BulletType findedBulletType = (BulletType)i;

                if (item.ItemInfo.ItemIndeficator.ToString() == findedBulletType.ToString())
                {
                    BulletInventory.InventoryAmmoSlot[i] = inventorySlot;

                    OnAmmoPickup?.Invoke(findedBulletType);
                    break;
                }
            }
        }

        _itemList.Add(inventorySlot);

    }

    private ItemSlotProperties SetProperties(ItemSlotProperties properties, Item item)
    {
        properties.Rarity = item.ItemInfo.Rarity;
        properties.ItemIndeficator = item.ItemInfo.ItemIndeficator;
        properties.ItemType = item.ItemInfo.ItemType;
        properties.Stacked = item.ItemInfo.Stacked;
        properties.PickupClip = item.ItemInfo.PickupClip;
        properties.Item = item.ItemInfo.Item;
        properties.ItemName = item.ItemInfo.ItemName;
        properties.ItemIcon = item.ItemInfo.ItemIcon;

        return properties;
    }

    public void ChoseItem(GameObject item, InventorySlot inventorySlot)
    {
        if (_chousedItem == item)
        {
            inventorySlot.SelectionFrame.SetActive(false);

            _chousedItem = null;

            UnchouseItem?.Invoke();

            return;
        }

        if (_chousedItem != null)
        {
            _chousedItem.GetComponent<InventorySlot>().SelectionFrame.SetActive(false);

            _chousedItem = item;

            inventorySlot.SelectionFrame.SetActive(true);

            ChouseItem?.Invoke(inventorySlot);

            return;

        }

        _chousedItem = item;

        inventorySlot.SelectionFrame.SetActive(true);

        ChouseItem?.Invoke(inventorySlot);

    }

    private void OnItemSlotDestroy(InventorySlot slot)
    {
        _itemList.Remove(slot);
    }

    private void OnDestroy()
    {
        InventorySlot.OnItemSlotDestroy -= OnItemSlotDestroy;
    }
}
