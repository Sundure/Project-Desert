using UnityEngine;

public class CameraRecoil : Recoil
{
    private void Start()
    {
        Weapon.OnRecoilFire += OnRecoil;
    }

    protected override void OnRecoil()
    {
        
    }

    private void OnRecoil(float recoil)
    {
        _targetRecoil = new Vector3(Random.Range(-recoil, -recoil * 5), Random.Range(-recoil, recoil), Random.Range(-_recoilZ, _recoilZ));
    }

    private void OnDestroy()
    {
        Weapon.OnRecoilFire -= OnRecoil;
    }
}
