using System.Collections;
using UnityEngine;

public class Pistol : Weapon
{
    [Header("Weapon")]
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Light _light;

    private void Particle()
    {
        _particleSystem.Play();

        _light.enabled = true;

        StartCoroutine(ParticleStop());
    }

    private IEnumerator ParticleStop()
    {
        yield return null;

        _particleSystem.Stop();

        yield return new WaitForSeconds(0.2f);

        _light.enabled = false;
    }

    override protected void OnEnable()
    {
        _light.enabled = false;

        base.OnEnable();

        OnFire += Particle;
    }

    override protected void OnDestroy()
    {
        base.OnDestroy();

        OnFire -= Particle;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnFire -= Particle;
    }
}
