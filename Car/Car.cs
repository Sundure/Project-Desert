using System;
using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
    public static event Action OnCarSeat;

    public float X;
    public float Y;

    public float RPMSpeed;
    public float Speed;
    public float WheelRotate;

    [SerializeField] private float _minSpeedMultiplier;

    [SerializeField] private int _maxSpeed;
    [SerializeField] private int _maxWheelRotate;

    [SerializeField] private int _speedBreak;
    [SerializeField] private int _standartBreakForce;
    public float BreakForce;

    private bool _break;
    private bool _canBreak;

    [SerializeField] private int _setSpeedMultiplier;

    private bool _used;

    private bool _tryEnable;
    private bool _activated;
    private bool _canActivate = true;

    [SerializeField] private Transform _carSeat;

    [SerializeField] private Transform _steeringWheel;

    [SerializeField] private CarWheel[] _carWheel;
    [SerializeField] private CarWheel[] _frontWheel;

    [SerializeField] private AudioClip _startCarClip;
    [SerializeField] private AudioClip _carEngineClip;
    [SerializeField] private AudioClip _carEngineSleepClip;

    [SerializeField] private AudioClip _idleClip;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _idleAudioSource;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;

        _idleAudioSource.loop = true;
        _idleAudioSource.clip = _idleClip;

        gameObject.layer = LayerMask.NameToLayer("Car");

        foreach (CarWheel wheel in _frontWheel)
        {
            wheel.FrontWheel = true;
        }
    }

    public void Activate()
    {
        if (_canActivate)
        {
            _idleAudioSource.Stop();

            _canActivate = false;

            _audioSource.loop = false;

            if (_activated)
            {
                _audioSource.volume = 1.0f;

                _activated = false;

                _audioSource.clip = _carEngineSleepClip;

                _audioSource.Play();
            }
            else if (_activated == false)
            {
                _audioSource.clip = _startCarClip;

                _tryEnable = true;

                _audioSource.Play();
            }

            StartCoroutine(KeyUse(_audioSource.clip.length));
        }
    }

    private IEnumerator KeyUse(float time)
    {
        yield return new WaitForSeconds(time);

        _canActivate = true;

        if (_tryEnable)
        {
            _idleAudioSource.Play();

            _tryEnable = false;

            _activated = true;

            _audioSource.volume = 0;
            _audioSource.clip = _carEngineClip;
            _audioSource.loop = true;

            _audioSource.Play();
        }
    }

    public void DoorUse(Transform exitPos)
    {
        _used = !_used;

        Player.Drived = _used;
        Player.CanMove = !_used;

        Player.ChangeGunEmbark(!_used);

        foreach (CarWheel wheel in _carWheel)
        {
            wheel.ActivateWheel(_used);
        }

        if (_used == false)
        {
            Player.Controller.transform.SetPositionAndRotation(exitPos.position, Quaternion.Euler(Vector3.zero));
        }
        else
        {
            OnCarSeat?.Invoke();
        }
    }

    private void Update()
    {
        float speedMultiplier = 2 - (Speed / _maxSpeed) * 2;
        float speedDownMultiplier = Speed / _maxSpeed * 2;

        speedDownMultiplier = Mathf.Clamp(speedDownMultiplier, _minSpeedMultiplier, 2);

        CarSpeed(speedDownMultiplier);

        if (_used)
        {
            X = Input.GetAxis("Horizontal");

            Rotate();

            Player.Controller.transform.SetPositionAndRotation(_carSeat.position, _carSeat.rotation);

            if (_activated)
            {
                Drive(speedMultiplier);
                Break();

                _audioSource.volume = Speed / _maxSpeed;
                _audioSource.volume = Mathf.Clamp(_audioSource.volume, 0f, 0.7f);

                Y = Input.GetAxis("Vertical");
            }
        }
    }

    private void CarSpeed(float speedDownMultiplier)
    {
        if (Speed > 0)
        {
            if (_activated == false)
            {
                Speed -= Time.deltaTime * 3 * speedDownMultiplier;

                if (Speed < 0.1)
                {
                    Speed = 0;
                }

                return;
            }

            float pedalStrenght = 1 - Y;

            pedalStrenght = Mathf.Clamp01(pedalStrenght);

            Speed -= Time.deltaTime * 3 * speedDownMultiplier * pedalStrenght;

            if (Speed < 0.1)
            {
                Speed = 0;
            }
        }
        else if (Speed < 0)
        {
            if (_activated == false)
            {
                Speed += Time.deltaTime * 2 * speedDownMultiplier;

                if (Speed > -0.1)
                {
                    Speed = 0;
                }
                return;
            }

            float pedalStrenght = 1 + Y;

            pedalStrenght = Mathf.Clamp01(pedalStrenght);

            Speed += Time.deltaTime * 2 * speedDownMultiplier * pedalStrenght;

            if (Speed > -0.1)
            {
                Speed = 0;
            }
        }
    }

    private void Drive(float speedMultiplier)
    {
        speedMultiplier = Mathf.Clamp(speedMultiplier, _minSpeedMultiplier, 2);

        Speed += Time.deltaTime * Y * _setSpeedMultiplier * speedMultiplier;
        Speed = Mathf.Clamp(Speed, -_maxSpeed / 3, _maxSpeed);
    }

    private void Rotate()
    {
        _steeringWheel.Rotate(Vector3.up, WheelRotate * 0.3f, Space.Self);

        if (X == 0)
        {
            WheelRotate = Mathf.Lerp(WheelRotate, 0, 1f);
        }
        else
        {
            WheelRotate = Mathf.Lerp(WheelRotate, _maxWheelRotate * X, 0.5f);
        }
    }

    private void Break()
    {
        if (Input.GetButton("Jump"))
        {
            BreakForce = _standartBreakForce;
            Speed -= Time.deltaTime * BreakForce / 10;

            Speed = Mathf.Clamp(Speed, 0, _maxSpeed);

            _break = true;
            _canBreak = true;
        }
        else if (_canBreak)
        {
            BreakForce = 0;

            _break = false;
            _canBreak = false;
        }

        if (Speed > 0 && Y < 0)
        {
            Speed -= _speedBreak * Time.deltaTime;
            BreakForce = _standartBreakForce / 10 * -Y;
        }
        else if (Speed < 0 && Y > 0)
        {
            Speed += _speedBreak * Time.deltaTime;
            BreakForce = _standartBreakForce / 10 * Y;
        }

        if (Speed < 0 && Y < 0 && _break == false)
        {
            BreakForce = 0;
        }
        else if (Speed > 0 && Y > 0 && _break == false)
        {
            BreakForce = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float breakSpeedMultiplier = Mathf.Clamp01(Speed / _maxSpeed);

        Speed *= breakSpeedMultiplier;
    }
}
