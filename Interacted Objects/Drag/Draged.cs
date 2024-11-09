using UnityEngine;

public class Draged : MonoBehaviour, IDraged
{
    private Rigidbody _rb;

    private bool _draged;

    private Transform _dragPoint;

    private readonly float _minSpeed = 2;
    private void Start()
    {
        enabled = false;

        if (TryGetComponent(out Rigidbody rb))
        {
            _rb = rb;
        }
    }

    private void Update()
    {
        if (_draged)
        {
            _rb.linearVelocity *= 0.9f;
            _rb.angularVelocity *= 0.9f;

            float speed = Vector3.Distance(transform.position, _dragPoint.position) * 2;

            if (speed < _minSpeed)
            {
                speed = _minSpeed;
            }

            transform.position = Vector3.MoveTowards(transform.position, _dragPoint.position, speed * Time.deltaTime);
        }
    }

    public void Drag(Transform transform)
    {
        this.transform.parent = transform;

        _dragPoint = transform;

        _rb.useGravity = false;

        _draged = true;

        enabled = true;
    }

    public void Drop()
    {
        transform.parent = null;

        _rb.useGravity = true;

        _draged = false;

        enabled = false;
    }

    public void Throw(Vector3 vector3, float power)
    {
        transform.parent = null;

        _rb.useGravity = true;

        _draged = false;

        enabled = false;

        _rb.AddExplosionForce(power, vector3, 1, 0, ForceMode.Force);
    }
}
