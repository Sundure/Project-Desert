using UnityEngine;

public class HitPoint : MonoBehaviour, IHitable, IFoced
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private ArmatureActivate _armatureActivate;
    [SerializeField] private float _damageMultiplier;
    public void TakeDamage(float damage)
    {
        _enemy.TakeDamage(damage * _damageMultiplier);
    }

    public void TakeForce(Vector3 vector3, Rigidbody rb)
    {
        _armatureActivate.ChangeForcePoint(vector3, rb);
    }
}
