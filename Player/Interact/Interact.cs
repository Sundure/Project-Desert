using System.Collections;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private void Update()
    {
        int layerMask = ~LayerMask.GetMask("Car","Player");

        Ray ray = _camera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, Player.InteractRange, layerMask))
        {
            StartCoroutine(FixedInteract(hit));
        }
    }

    private IEnumerator FixedInteract(RaycastHit hit)
    {
        yield return null;

        if (Input.GetButtonDown("Interact"))
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteracted interacted))
            {
                interacted.Interact();
            }
            else if (hit.collider.gameObject.TryGetComponent(out IPickuble pickable))
            {
                pickable.Pickup();
            }
        }
    }
}

