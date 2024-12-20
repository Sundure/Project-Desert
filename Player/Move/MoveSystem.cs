using UnityEngine;

public class MoveSystem : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;

    [SerializeField] private Player _player;

    [SerializeField] private Transform _footTrasform;

    [SerializeField] private LayerMask _groundLayer;

    private Vector3 _height;

    private void Awake()
    {
        Player.Controller = _characterController;
    }

    private void Update()
    {
        if (Player.CanMove)
        {

            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            _player.Grounded = Physics.CheckSphere(_footTrasform.position, _player.GravityCheckDistance, _groundLayer);


            Vector3 vector3 = transform.right * x + transform.forward * y;

            if (_player.Grounded)
            {
                _player.FirstJump = true;
                _player.SecondJump = true;

                _height = Vector3.zero;
            }
            else if (_player.Grounded == false)
            {
                _height.y += _player.Gravity * Time.deltaTime;

                Player.Controller.Move(_height * Time.deltaTime);

                _player.FirstJump = false;
            }

            if (_player.Jump == true)
            {
                if (_player.JumpTime <= _player.JumpVelocity / 5)
                    _player.Jump = false;
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (_player.FirstJump == true)
                {
                    _player.FirstJump = false;

                    _player.Jump = true;

                    _player.JumpTime = _player.JumpVelocity;
                }
                else if (_player.SecondJump == true)
                {
                    _player.SecondJump = false;

                    _player.Jump = true;

                    _player.JumpTime = _player.JumpVelocity * 1.5f;
                }
            }

            Player.Controller.Move(_player.PlayerSpeed * Time.deltaTime * vector3);
        }
    }

    private void FixedUpdate()
    {
        if (_player.Jump == true)
        {
            _height.y = Mathf.Sqrt(_player.JumpStrenght * Time.fixedDeltaTime * 2);

            Player.Controller.Move(Time.fixedDeltaTime * _player.JumpTime * _height);

            _player.JumpTime -= Time.fixedDeltaTime * 3;
        }
    }
}
