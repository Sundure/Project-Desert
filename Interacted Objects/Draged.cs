using UnityEngine;

public class Draged : MonoBehaviour, IDraged
{
    private Rigidbody _rb;

    private void Start()
    {
        if (TryGetComponent(out Rigidbody rb))
        {
            _rb = rb;
        }
    }
    public void Drag(Transform transform)
    {
        this.transform.parent = transform;

        _rb.useGravity = false;
        //_rb.isKinematic = true;
    }

    public void Drop()
    {
        transform.parent = null;

        _rb.useGravity = true;
        _rb.isKinematic = false;
    }
}
