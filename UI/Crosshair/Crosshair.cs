using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private void Start()
    {
        Weapon.ChangeAim += ChangeCrosshair;
    }

    private void ChangeCrosshair(bool enabled)
    {
        gameObject.SetActive(!enabled);
    }

    private void OnDestroy()
    {
        Weapon.ChangeAim -= ChangeCrosshair;
    }
}
