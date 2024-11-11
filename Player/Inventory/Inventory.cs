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
        int count = 0;

        for (int i = 0; i < _itemList.Count; i++)
        {
            InventorySlot findedItem = _itemList[i];

            if (findedItem.ItemIndeficator == item.ItemInfo.ItemIndeficator && findedItem.Stacked == true)
            {
                findedItem.ItemCount += item.ItemCount;
                findedItem.Weight += item.ItemInfo.Weight;

                findedItem.UpdateUIItemCount();

                return;
            }
        }

        GameObject newSlot = Instantiate(_inventorySlotPrefab, Vector3.zero, Quaternion.identity, _itemSlot.transform);

        InventorySlot itemSlot = newSlot.GetComponent<InventorySlot>();

        itemSlot.Rarity = item.ItemInfo.Rarity;
        itemSlot.ItemIndeficator = item.ItemInfo.ItemIndeficator;
        itemSlot.ItemType = item.ItemInfo.ItemType;
        itemSlot.Stacked = item.ItemInfo.Stacked;
        itemSlot.PickupClip = item.ItemInfo.PickupClip;
        itemSlot.Item = item.ItemInfo.Item;
        itemSlot.SlotNumber = count;
        itemSlot.ItemCount = item.ItemCount;
        itemSlot.Inventory = this;
        itemSlot.ItemName = item.ItemInfo.ItemName;

        if (item.ItemInfo.ItemIcon != null)
        {
            itemSlot.ItemIcon = item.ItemInfo.ItemIcon;
        }

        if (item.ItemInfo.ItemType == ItemTypes.Ammo)
        {
            for (int i = 0; i < Enum.GetValues(typeof(BulletType)).Length; i++)
            {
                BulletType findedBulletType = (BulletType)i;

                if (item.ItemInfo.ItemIndeficator.ToString() == findedBulletType.ToString())
                {
                    Debug.Log("Bullet Finded");

                    break;
                }
            }
        }

        _itemList.Add(itemSlot);

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
}
