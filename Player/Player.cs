using UnityEngine;

public class Player : MonoBehaviour
{
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
}
