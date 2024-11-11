using UnityEngine;

public class DropButton : MonoBehaviour
{
    private InventorySlot _item;

    private void Awake()
    {
        Inventory.ChouseItem += GetItem;
        Inventory.UnchouseItem += DisableButton;
        InventorySlot.OnItemSlotDestroy += OnItemSlotDestroy;

        gameObject.SetActive(false);

    }

    private void GetItem(InventorySlot item)
    {
        _item = item;

        gameObject.SetActive(true);
    }

    public void Drop()
    {
        Debug.Log(_item.ItemType);

        _item.Drop();

    }

    private void DisableButton()
    {
        _item = null;

        gameObject.SetActive(false);
    }

    private void OnItemSlotDestroy(InventorySlot slot)
    {
        if (slot == _item)
        {
            DisableButton();
        }
    }

    private void OnDestroy()
    {
        Inventory.ChouseItem -= GetItem;
        Inventory.UnchouseItem -= DisableButton;
        InventorySlot.OnItemSlotDestroy -= OnItemSlotDestroy;
    }
}
