using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private bool _dragging;

    [SerializeField] private Transform _dragPointTransform;

    private IDraged _draged;
    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Ray ray = _camera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

            Physics.Raycast(ray, out RaycastHit hit, Player.InteractRange);

            if (_dragging == false)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent(out IDraged draged))
                    {
                        _draged = draged;

                        _dragging = true;
                    }
                }
            }
        }

        if (Input.GetButton("Interact") && _dragging)
        {
            Ray ray = _camera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

            Physics.Raycast(ray, out RaycastHit hit, Player.InteractRange);

            if (Input.GetButtonDown("Fire1"))
            {
                _draged.Throw(hit.point);

                _dragging = false;
            }

            if (_dragging)
            {
                _draged.Drag(_dragPointTransform);
            }
        }
        else if (Input.GetButtonUp("Interact") && _draged != null)
        {
            {
                _dragging = false;
                _draged.Drop();

                _draged = null;
            }
        }
    }
}
