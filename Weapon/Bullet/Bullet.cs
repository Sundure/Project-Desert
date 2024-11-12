using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 _targetPos;

    [SerializeField] private float _speed;

    [SerializeField] private float _lifeTime;

    private void Update()
    {
        _lifeTime -= Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, _targetPos, _speed * Time.deltaTime);

        if (transform.position == _targetPos)
        {
            Destroy(gameObject);
        }

        if (_lifeTime <= 0)
        {
            Destroy(gameObject);
        }

    }

    public void ChangeTarget(Vector3 vector3)
    {
        _targetPos = vector3;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
