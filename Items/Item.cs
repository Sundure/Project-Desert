using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemInfo ItemInfo;

    public int ItemCount = 1;
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
