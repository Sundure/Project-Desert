using UnityEngine;

public class MoveSystem : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;

    [SerializeField] private Player _player;

    [SerializeField] private bool _grounded;
    [SerializeField] private bool _jump;

    [SerializeField] private int _fallStrenght;

    [SerializeField] private float _jumpStrenght;
    [SerializeField] private float _jumpSpeed;

    [SerializeField] private float _fallSpeed;

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 vector3 = transform.right * x + transform.forward * y;

        _grounded = Physics.Raycast(transform.position, Vector3.down, 1.3f);

        if (_grounded && _jump == false)
        {
            _fallSpeed = 0;
        }

        if (_jump == true)
        {
            _fallSpeed += Time.deltaTime / _jumpStrenght;

            vector3.y += _fallSpeed * -Time.deltaTime;

            if (_fallSpeed >= -_jumpSpeed * _jumpSpeed)
            {
                _jump = false;
            }
        }
        else if (_grounded == false)
        {
            _fallSpeed += Time.deltaTime * _fallStrenght * 10;

            vector3.y += _fallSpeed * -Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _jump == false)
        {
            _jump = true;

            _fallSpeed = -_fallStrenght * _jumpSpeed;
        }

        _characterController.Move(_player.PlayerSpeed * Time.deltaTime * vector3);
    }
}
