using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private bool _changeCrosshair;

    [SerializeField] private RawImage _image;

    private void Start()
    {
        Weapon.ChangeAim += ChangeCrosshair;

        enabled = false;
    }

    private void ChangeCrosshair(bool booled)
    {
        booled = !booled;

        _changeCrosshair = booled;

        enabled = true;
    }

    private void Update()
    {
        if (_changeCrosshair == false)
        {
            Color color = _image.color;

            color.a -= Time.deltaTime * 2;

            _image.color = color;

            if (_image.color.a <= 0)
            {
                enabled = false;
            }
        }
        else
        {
            Color color = _image.color;

            color.a += Time.deltaTime * 2;

            _image.color = color;

            if (_image.color.a >= 1)
            {
                enabled = false;
            }
        }
    }

    private void OnDestroy()
    {
        Weapon.ChangeAim -= ChangeCrosshair;
    }
}
