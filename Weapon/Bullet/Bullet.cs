using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 _targetPos;

    public float Damage;

    [SerializeField] private float _speed;

    [SerializeField] private float _lifeTime;

    private void Update()
    {
        _lifeTime -= Time.deltaTime;

        if (_targetPos == Vector3.zero)
        {
            transform.position += _speed * Time.deltaTime * transform.right;

            if (_lifeTime < 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, _speed * Time.deltaTime);

            if (transform.position == _targetPos)
            {
                transform.position += _speed * Time.deltaTime * transform.right;

                _targetPos = Vector3.zero;
            }

            if (_lifeTime < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void ChangeTarget(Vector3 vector3)
    {
        _targetPos = vector3;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out IFoced hitable))
        {
            hitable.TakeForce(transform.position, Damage);
        }

        if (collision.collider.gameObject.TryGetComponent(out IHitable iHit))
        {
            iHit.TakeDamage(Damage);
        }

        Destroy(gameObject);
    }
}
