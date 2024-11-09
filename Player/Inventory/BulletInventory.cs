using System;
using UnityEngine;
public class BulletInventory : MonoBehaviour
{
    public static int[] Ammo = new int[Enum.GetValues(typeof(BulletType)).Length];

    public static string[] AmmoName = new string[Enum.GetValues(typeof(BulletType)).Length];

    private void Start()
    {
        for (int i = 0; i < AmmoName.Length; i++)
        {
            BulletType bulletType = (BulletType)i;

            AmmoName[i] = bulletType.ToString();
        }
    }
}
