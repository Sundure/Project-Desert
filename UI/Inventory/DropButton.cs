using UnityEngine;

public class DropButton : MonoBehaviour
{
    private InventorySlot _item;

    private void Awake()
    {
        Inventory.ChouseItem += GetItem;
        Inventory.UnchouseItem += DisableButton;

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
    }

    private void DisableButton()
    {
        _item = null;

        gameObject.SetActive(false);
    }
}
