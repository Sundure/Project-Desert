using UnityEngine;

public class SpawnBullets : MonoBehaviour
{
    [SerializeField] private GameObject _spawnBullet;

    private Bullet _bullet;

    private void SpawnBullet(Transform transform, float damage)
    {
        Quaternion rotation = Quaternion.Euler(transform.eulerAngles.x + 90, transform.eulerAngles.y, transform.eulerAngles.z + 90);

        GameObject newBullet = Instantiate(_spawnBullet, transform.position, rotation);

        if (newBullet.TryGetComponent(out Bullet bullet))
        {
            _bullet = bullet;
            _bullet.Damage = damage;
        }
    }

    private void ChangeBulletTarget(Vector3 vector3)
    {
        _bullet.ChangeTarget(vector3);
    }

    private void OnDisable()
    {
        Weapon.SpawnBullet -= SpawnBullet;
        Weapon.OnHit -= ChangeBulletTarget;
    }
    private void OnEnable()
    {
        Weapon.SpawnBullet += SpawnBullet;
        Weapon.OnHit += ChangeBulletTarget;
    }
    private void OnDestroy()
    {
        Weapon.SpawnBullet -= SpawnBullet;
        Weapon.OnHit -= ChangeBulletTarget;
    }
}
