using System.Collections;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private int _layerMask;

    [SerializeField] private Inventory _inventory;
    private void Start()
    {
        _layerMask = ~LayerMask.GetMask("Car", "Player");
    }
    private void Update()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        if (Input.GetButtonDown("Interact") && Player.CanInteract)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Player.InteractRange, _layerMask) && hit.collider.gameObject != null)
            {
                StartCoroutine(FixedInteract(hit));
            }
        }
    }

    private IEnumerator FixedInteract(RaycastHit hit)
    {
        yield return null;

        if (hit.collider.gameObject.TryGetComponent(out IInteracted interacted))
        {
            interacted.Interact();
        }
        else if (hit.collider.gameObject.TryGetComponent(out IPickuble pickable))
        {
            pickable.Pickup();
        }
        else if (hit.collider.gameObject.TryGetComponent(out Item item))
        {
            _inventory.AddItem(item);

            item.Pickup();
        }
    }
}

