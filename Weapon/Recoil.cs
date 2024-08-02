using UnityEngine;

public class Recoil : MonoBehaviour
{
    private Vector3 _position;
    protected Vector3 _targetRecoil;

    [SerializeField] protected float _recoilX;
    [SerializeField] protected float _recoilY;
    [SerializeField] protected float _recoilZ;

    [SerializeField] private float _returnSpeed;
    [SerializeField] private float _speed;

    private void Update()
    {
        _targetRecoil = Vector3.Lerp(_targetRecoil, Vector3.zero, _returnSpeed * Time.deltaTime);

        _position = Vector3.Slerp(_position, _targetRecoil, _speed * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(_position);
    }

    virtual protected void OnRecoil()
    {
        _targetRecoil = new Vector3(Random.Range(-_recoilX, -_recoilX), Random.Range(-_recoilY, _recoilY), Random.Range(-_recoilZ, _recoilZ));
    }
}
