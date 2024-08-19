using UnityEngine;

public class SpawnBullets : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;

    private void SpawnBullet(Transform transform)
    {
        Quaternion rotation = Quaternion.Euler(transform.eulerAngles.x + 90, transform.eulerAngles.y, transform.eulerAngles.z + 90);

        Instantiate(_bullet, transform.position, rotation);
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
