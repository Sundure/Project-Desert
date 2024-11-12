using System;
using UnityEngine;
public class BulletInventory : MonoBehaviour
{
    public static InventorySlot[] InventoryAmmoSlot = new InventorySlot[Enum.GetValues(typeof(BulletType)).Length];

    public static int TakeAmmo(int ammoIndex, int requestAmmo)
    {
        InventorySlot ammoSlot = InventoryAmmoSlot[ammoIndex];

        int ammo;

        if (requestAmmo > ammoSlot.ItemCount)
        {
            ammo = ammoSlot.ItemCount;

            ammoSlot.ChangeItemCount(-ammoSlot.ItemCount);
        }
        else
        {
            ammo = requestAmmo;

            ammoSlot.ChangeItemCount(-requestAmmo);
        }

        ammoSlot.UpdateUIItemCount();

        return ammo;
    }
}
