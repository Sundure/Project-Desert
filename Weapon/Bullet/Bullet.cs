using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 _targetPos;

    [SerializeField] private float _speed;

    [SerializeField] private float _lifeTime;

    private void Update()
    {
        _lifeTime -= Time.deltaTime;

        transform.position += _speed * Time.deltaTime * transform.right;

        if (transform.position == _targetPos)
        {
            Destroy(gameObject);
        }
        else if (_lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }
}
