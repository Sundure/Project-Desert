using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private ArmatureActivate _armatureActivate;

    private float _healt = 100;
    private bool _alive = true;

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

                _alive = false;
            }
        }
    }

    private void Die(float damage)
    {
        _armatureActivate.ActivateRagdoll();
        _armatureActivate.Force(damage);
    }
}
