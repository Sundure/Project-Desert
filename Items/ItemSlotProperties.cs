using UnityEngine;

public class ItemSlotProperties : ScriptableObject
{
    public ItemRarity Rarity;
    public ItemList ItemIndeficator;
    public ItemTypes ItemType;

    public bool Stacked;

    public AudioClip[] PickupClip;

    public GameObject Item;

    public string ItemName;

    public Texture ItemIcon;    
}
