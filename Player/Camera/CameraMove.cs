using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Player _player;

    private float _xRotation;
    private float _yRotation;

    public float XRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Car.OnCarSeat += ChangeZeroCameraPos;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _player.MouseSens * _player.SensMultiplier * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _player.MouseSens * _player.SensMultiplier * Time.deltaTime;

        _xRotation -= mouseY;

        if (Player.Drived == false)
        {
            _xRotation = Mathf.Clamp(_xRotation, -_player.MaxCameraUpRotation, _player.MaxCameraDownRotation);

            transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

            _playerTransform.transform.Rotate(Vector3.up * mouseX);

            XRotation = transform.localEulerAngles.x;
        }
        else
        {
            _yRotation += mouseX;

            _xRotation = Mathf.Clamp(_xRotation, -30, 30);

            transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        }
    }

    private void ChangeZeroCameraPos()
    {
        _xRotation = 0;
        _yRotation = 0;
    }

    private void OnDestroy()
    {
        Car.OnCarSeat -= ChangeZeroCameraPos;
    }
}
