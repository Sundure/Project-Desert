using UnityEngine;

public class Forced : MonoBehaviour, IFoced
{
    private Rigidbody _rb;

    private void Start()
    {
         _rb = GetComponent<Rigidbody>();
    }

    public void TakeForce(Vector3 vector3, float force)
    {
        vector3 = (_rb.position - vector3).normalized;

        _rb.AddForce(force * 3 * vector3);
    }
}