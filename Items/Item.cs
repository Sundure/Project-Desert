using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Data")]

    public ItemInfo ItemInfo;

    [Header("Dinamic Values")]

    public int ItemCount = 1;

    private void Start()
    {
        if (ItemInfo.HasCollisionWithPlayer == false)
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore Player");
        }
    }

    public void Pickup()
    {
        if (ItemInfo.PickupClip.Length > 0)
        {
            int random = Random.Range(0, ItemInfo.PickupClip.Length);

            PickubleAudioSource.AudioSource.PlayOneShot(ItemInfo.PickupClip[random]);
        }

        Destroy(gameObject);
    }

}
