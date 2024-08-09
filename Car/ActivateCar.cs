using UnityEngine;

public class ActivateCar : MonoBehaviour, IInteracted
{
    [SerializeField] private Car _car;
    public void Interact()
    {
        _car.Activate();
    }
}
