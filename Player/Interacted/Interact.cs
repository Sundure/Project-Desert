using System.Collections;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private void Update()
    {
            int layerMask = ~LayerMask.GetMask("Car");

            Ray ray = _camera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

            if (Physics.Raycast(ray, out RaycastHit hit, 3, layerMask))
            {
                StartCoroutine(FixedInterat(hit));
            }
    }

    private IEnumerator FixedInterat(RaycastHit hit)
    {
        yield return null;

        if (Input.GetButtonDown("Interact"))
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteracted interacted))
            {
                interacted.Interact();
            }
        }
    }
}

