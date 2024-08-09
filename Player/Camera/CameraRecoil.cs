using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    private Vector3 _position;
    protected Vector3 _targetRecoil;

    [SerializeField] private float _returnSpeed;
    [SerializeField] private float _speed;
    private void Start()
    {
        Weapon.OnRecoilFire += OnRecoil;
    }
    private void Update()
    {
        _targetRecoil = Vector3.Lerp(_targetRecoil, Vector3.zero, _returnSpeed * Time.deltaTime);
        _position = Vector3.Slerp(_position, _targetRecoil, _speed * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(_position);
    }

    private void OnRecoil(float recoil)
    {
        _targetRecoil = new Vector3(Random.Range(-recoil, -recoil * 5), Random.Range(-recoil, recoil), 0);
    }

    private void OnDestroy()
    {
        Weapon.OnRecoilFire -= OnRecoil;
    }
}
