using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
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

    public int ItemCount;

    public AudioClip[] PickupClip;

    [Header("Item")]

    public GameObject HandItem;

    public GameObject GroundItem;

    public int SlotNumber;

    [Header("UI")]

    public Image ItemIcon;

    public GameObject SelectionFrame;

    [Header("Other")]

    [SerializeField] private InventorySlotUI _inventorySlotUI;

    [SerializeField] private GameObject _panel;

    public Inventory Inventory;

    private void Start()
    {
        if (gameObject.TryGetComponent(out InventorySlotUI slotUI))
        {
            _inventorySlotUI = slotUI;

            _inventorySlotUI.ItemName.text = ItemType.ToString();

            UpdateItemCount();
        }
    }

    public void UpdateItemCount()
    {
        if (ItemCount <= 1)
        {
            return;
        }
        else
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
        Instantiate(GroundItem);

        if (ItemCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnItemClick()
    {
        Inventory.ChoseItem(gameObject,this);
    }
}
