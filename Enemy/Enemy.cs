using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private ArmatureActivate _armatureActivate;

    private float _healt = 100;

    public void TakeDamage(float damage)
    {
        Debug.Log($"I Take Damage {gameObject.name}");

        if (_healt > 0)
        {
            _healt -= damage;

            Debug.Log(_healt);

            if (_healt <= 0)
            {
                Die(damage);
            }
        }
    }

    private void Die(float damage)
    {
        _armatureActivate.ActivateRagdoll(damage);
    }
}
