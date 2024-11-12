using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static LayerMask PlayerLayer {  get; private set; }

    [Header("Controler")]
    public static CharacterController Controller;

    [Header("Stats")]
    public float PlayerSpeed;

    [Header("Jump Stats")]
    public float JumpForce;
    public float JumpStrenght;
    public float JumpVelocity;
    public float JumpTime;

    [Header("Jumps")]
    public bool Jump;

    public bool FirstJump;
    public bool SecondJump;

    [Header("Gravity")]
    public float FallMultiplier;

    public float Gravity;

    public float GravityCheckDistance;

    public bool Grounded;
    [Header("Gun")]
    public static bool Aiming;
    public static bool CanUseGun = true;

    [Header("Booled")]
    public static bool CanMove = true;
    public static bool CanInteract = true;
    public static bool Drived;
    public static bool UseInventory;
    public static bool Dragging;

    [Header("Interact")]
    public static readonly float InteractRange = 3.5f;


    public static event Action ChangeAim;

    private void Start()
    {
        PlayerLayer = gameObject.layer;
    }

    public static void ChangeGunEmbark(bool booled)
    {
        CanUseGun = booled;

        ChangeAim?.Invoke();
    }

    public static void AutomaticChangeGunEmbark()
    {
        if (Drived || UseInventory || Dragging)
        {
            CanUseGun = false;
        }
        else
        {
            CanUseGun = true;
        }
    }
}
