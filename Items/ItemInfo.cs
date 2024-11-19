using UnityEngine;

[CreateAssetMenu(menuName = "Scripts/Items/ItemData")]
public class ItemInfo : ScriptableObject
{

    [Header("Stats")]

    [SerializeField] private float _weight;
    public float Weight { get { return _weight; } }

    [Header("Rarity")]

    [SerializeField] private ItemRarity _rarity;
    public ItemRarity Rarity { get { return _rarity; } }

    [Header("Type")]

    [SerializeField] private ItemList _itemIndeficator;
    public ItemList ItemIndeficator { get { return _itemIndeficator; } }

    [SerializeField] private ItemTypes _itemType;
    public ItemTypes ItemType { get { return _itemType; } }

    [Header("Parametrs")]

    [SerializeField] private bool _stacked;
    public bool Stacked { get { return _stacked; } }

    [SerializeField] private AudioClip[] _pickupClip;
    public AudioClip[] PickupClip { get { return _pickupClip; } }

    [Header("Icon")]

    [SerializeField] private Texture _itemIcon;
    public Texture ItemIcon { get { return _itemIcon; } }

    [Header("Item")]

    [SerializeField] private string _itemName;
    public string ItemName { get { return _itemName; } }

    [SerializeField] private GameObject _item;
    public GameObject Item { get { return _item; } }

    [Header("Other")]

    [SerializeField] private bool _hasCollisionWithPlayer;
    public bool HasCollisionWithPlayer { get { return _hasCollisionWithPlayer; } }
}
