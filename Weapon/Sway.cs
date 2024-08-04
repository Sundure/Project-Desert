using UnityEngine;

public class Sway : MonoBehaviour
{
    [SerializeField] private float _returnSpeed;
    [SerializeField] private float _speed;

    private Vector3 _position;
    private Vector3 _targetPosition;

    private void Update()
    {
        float x = Input.GetAxis("Mouse Y");
        float y = Input.GetAxis("Mouse X");

        _position += new Vector3(x, -y, 0) * _speed;

        _targetPosition = Vector3.Lerp(_targetPosition, Vector3.zero, Time.deltaTime);

        _position = Vector3.Slerp (_position, _targetPosition, _returnSpeed * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(_position);
    }
}
