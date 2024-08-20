using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Controler")]
    public static CharacterController Controller;

    [Header("Camera")]
    public float MouseSens;
    public float SensMultiplier;

    public readonly float MaxCameraUpRotation = 80;
    public readonly float MaxCameraDownRotation = 90;

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
    public static bool Drived;

    public static void ChangeGunEmbark(bool booled)
    {
        CanUseGun = booled;
    }
}
