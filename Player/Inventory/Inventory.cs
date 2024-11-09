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

            Pause.PauseGame();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Pause.UnpauseGame();
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

            if (findedItem.ItemType == item.ItemType && findedItem.Stacked == true)
            {
                findedItem.ItemCount += item.ItemCount;
                findedItem.Weight += item.Weight;

                findedItem.UpdateItemCount();

                return;
            }
        }

        GameObject newSlot = Instantiate(_inventorySlotPrefab, Vector3.zero, Quaternion.identity, _itemSlot.transform);

        InventorySlot itemSlot = newSlot.GetComponent<InventorySlot>();

        itemSlot.Rarity = item.Rarity;
        itemSlot.ItemType = item.ItemType;
        itemSlot.Resource = item.Resource;
        itemSlot.Ammo = item.Ammo;
        itemSlot.Weapon = item.Weapon;
        itemSlot.Stacked = item.Stacked;
        itemSlot.PickupClip = item.PickupClip;
        itemSlot.ItemIcon = item.ItemIcon;
        itemSlot.HandItem = item.HandItem;
        itemSlot.GroundItem = item.GroundItem;
        itemSlot.SlotNumber = count;
        itemSlot.ItemCount = item.ItemCount;
        itemSlot.Inventory = this;

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
            _chousedItem.SetActive(false);
        }

        _chousedItem = item;

        inventorySlot.SelectionFrame.SetActive(true);

        ChouseItem?.Invoke(inventorySlot);

    }
}