using System.Collections;
using UnityEngine;

public class ArmatureActivate : MonoBehaviour
{
    [SerializeField] private Rigidbody[] _rig;

    [SerializeField] private Animator _animator;

    private Vector3 _position;
    private Rigidbody _rb;

    private float _forceMultiplier;

    private const float _constForceMultipier = 25;

    private void Start()
    {
        foreach (Rigidbody rig in _rig)
        {
            rig.useGravity = false;
            rig.isKinematic = true;
        }
    }

    public void ActivateRagdoll(float force)
    {
        StartCoroutine(DisableAnim());
        RagdolForce(force);
    }

    private IEnumerator DisableAnim()
    {
        _animator.SetTrigger("Exit");

        foreach (Rigidbody rig in _rig)
        {
            rig.isKinematic = false;
            rig.useGravity = true;
        }

        yield return new WaitForSeconds(0.5f);

        _animator.enabled = false;

    }

    public void ChangeForcePoint(Vector3 vector3, Rigidbody rb, float force)
    {
        _position = vector3;
        _rb = rb;
        _forceMultiplier = force;
    }

    private void RagdolForce(float force)
    {
        Vector3 vector3 = (_rb.position - _position).normalized;

        _rb.AddForce(force * _forceMultiplier * _constForceMultipier * vector3);
    }
}
