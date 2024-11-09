using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField] private float _throwPower;

    [SerializeField] private Camera _camera;

    [SerializeField] private Transform _dragPointTransform;

    private IDraged _draged;
    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            Ray ray = _camera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

            Physics.Raycast(ray, out RaycastHit hit, Player.InteractRange);

            if (Player.Dragging == false)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent(out IDraged draged))
                    {
                        _draged = draged;

                        Player.Dragging = true;

                        Player.Aiming = false;
                        Player.AutomaticChangeGunEmbark();
                    }
                }
            }
        }

        if (Input.GetButton("Interact") && Player.Dragging)
        {
            Ray ray = _camera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

            Physics.Raycast(ray, out RaycastHit hit, Player.InteractRange);

            if (Input.GetButtonDown("Fire1"))
            {
                _draged.Throw(hit.point , _throwPower);

                Player.Dragging = false;
            }

            if (Player.Dragging)
            {
                _draged.Drag(_dragPointTransform);
            }
        }
        else if (Input.GetButtonUp("Interact") && _draged != null)
        {
            {
                Player.Dragging = false;
                _draged.Drop();

                _draged = null;

                Player.AutomaticChangeGunEmbark();
            }
        }
    }
}
