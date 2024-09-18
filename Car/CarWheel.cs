using UnityEngine;

public class CarWheel : MonoBehaviour
{
    public bool FrontWheel;
    private bool _work;

    [SerializeField] private Car _car;

    [SerializeField] private Transform _wheel;

    [SerializeField] private WheelCollider _wheelCollider;

    private void Start()
    {
        enabled = false;
    }

    public void ActivateWheel(bool booled)
    {
        enabled = booled;
        _work = booled;
    }

    private void Update()
    {
        if (_work)
        {
            _car.RPMSpeed = _wheelCollider.rpm;

            _wheelCollider.brakeTorque = _car.BreakForce;

            _wheel.Rotate(Vector3.right * _car.Speed / 5);

            _wheelCollider.motorTorque = (_car.Speed / 10);

            if (FrontWheel)
            {
                _wheelCollider.steerAngle = _car.WheelRotate;
            }
        }
    }
}
