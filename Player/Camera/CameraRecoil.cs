using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    [SerializeField] private Vector3 _position;
    [SerializeField] private Vector3 _targetRecoil;

    [SerializeField] private float _returnSpeed;
    [SerializeField] private float _speed;

    [SerializeField] private CameraMove _cameraMove;
    [SerializeField] private Player _player;

    private const float _minTriggerRotationValue = 270;
    private const float _maxTriggerRotationValue = 360;
    private float _maxRotationZone = 90;
    private const float _minRotationZone = 0;
    private void Start()
    {
        _maxRotationZone = _player.MaxCameraUpRotation;

        Weapon.OnRecoilFire += OnRecoil;
    }
    private void Update()
    {
        _targetRecoil = Vector3.Lerp(_targetRecoil, Vector3.zero, _returnSpeed * Time.deltaTime);

        _position = Vector3.Slerp(_position, _targetRecoil, _speed * Time.deltaTime);

        RecoilLimit();

        transform.localRotation = Quaternion.Euler(-_position);
    }

    private void RecoilLimit()
    {
        float maxCamRotation = _player.MaxCameraUpRotation;

        float rotation = _cameraMove.XRotation;
        if (rotation > maxCamRotation)
        {
            rotation = maxCamRotation;
        }

        float maxRotation = rotation + maxCamRotation;

        maxRotation = Mathf.Clamp(maxRotation, 0, maxCamRotation + _player.MaxCameraDownRotation);

        if (_cameraMove.XRotation >= _minTriggerRotationValue && _cameraMove.XRotation <= _maxTriggerRotationValue)
        {
            float normalized = (_cameraMove.XRotation - _minTriggerRotationValue) / (_maxTriggerRotationValue - _minTriggerRotationValue);

            float angle = normalized * (_maxRotationZone - _minRotationZone) + _minRotationZone;

            maxRotation = angle;

            Debug.Log(maxRotation);
        }

        _position.x = Mathf.Clamp(_position.x, 0, maxRotation);

        if (maxRotation == 0)
        {
            Debug.Log("Limit");
        }
    }

    private void OnRecoil(float recoil)
    {
        _targetRecoil = new Vector3(Random.Range(recoil, recoil * 5), Random.Range(recoil, -recoil), 0);
    }

    private void OnDestroy()
    {
        Weapon.OnRecoilFire -= OnRecoil;
    }
}
