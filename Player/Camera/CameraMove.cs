using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform _player;

    [Header("Camera Move")]

    [SerializeField] private float _mouseSens;

    [SerializeField] private int _multiplier;

    private float _xRotation;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSens * _multiplier * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSens * _multiplier * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

        _player.transform.Rotate(Vector3.up * mouseX);
    }
}
