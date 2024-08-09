using UnityEngine;

public class CarDoor : MonoBehaviour, IInteracted
{
    [SerializeField] Car _car;

    [SerializeField] private Transform _exitPos;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _doorClip;

    public void Interact()
    {
        _audioSource.PlayOneShot(_doorClip);

        _car.DoorUse(_exitPos);
    }
}
