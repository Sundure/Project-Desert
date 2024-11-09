using UnityEngine;

public class HitPoint : MonoBehaviour, IHitable, IFoced
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private ArmatureActivate _armatureActivate;

    [SerializeField] private float _damageMultiplier;
    [SerializeField] private float _forceMultiplier;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(float damage)
    {
        _enemy.TakeDamage(damage * _damageMultiplier);
    }

    public void TakeForce(Vector3 vector3, float force)
    {
        _armatureActivate.ChangeForcePoint(vector3, _rb, _forceMultiplier);
    }
}
