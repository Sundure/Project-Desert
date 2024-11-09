using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private CameraSettings _cameraSettings;

    private float _xRotation;
    private float _yRotation;

    public float XRotation;

    public static Camera Camera;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Car.OnCarSeat += ChangeZeroCameraPos;

        Camera = GetComponent<Camera>();
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _cameraSettings.MouseSens * _cameraSettings.SensMultiplier * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _cameraSettings.MouseSens * _cameraSettings.SensMultiplier * Time.deltaTime;

        _xRotation -= mouseY;

        if (Player.Drived == false)
        {
            _xRotation = Mathf.Clamp(_xRotation, -_cameraSettings.MaxUpRotation, _cameraSettings.MaxDownRotation);

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
