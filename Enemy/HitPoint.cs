using UnityEngine;

public class HitPoint : MonoBehaviour , IHitable
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private float _damageMultiplier;
    public void TakeDamage(float damage)
    {
        _enemy.TakeDamage(damage * _damageMultiplier);
    }
}
