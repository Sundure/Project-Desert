using UnityEngine;

public class SpawnBullets : MonoBehaviour
{
    [SerializeField] private GameObject _spawnBullet;

    private void SpawnBullet(Transform transform, Vector3 vector3)
    {
        Quaternion rotation = Quaternion.Euler(transform.eulerAngles.x + 90, transform.eulerAngles.y, transform.eulerAngles.z + 90);

        GameObject newBullet = Instantiate(_spawnBullet, transform.position, rotation);

        if (newBullet.TryGetComponent(out Bullet bullet))
            bullet.ChangeTarget(vector3);

    }

    private void OnDisable()
    {
        Weapon.SpawnBullet -= SpawnBullet;
    }
    private void OnEnable()
    {
        Weapon.SpawnBullet += SpawnBullet;
    }
    private void OnDestroy()
    {
        Weapon.SpawnBullet -= SpawnBullet;
    }
}
