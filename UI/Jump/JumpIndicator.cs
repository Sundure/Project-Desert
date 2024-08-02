using UnityEngine;

public class JumpIndicator : MonoBehaviour
{
    [SerializeField] private Player _player;

    [SerializeField] private GameObject _firstJumpIndicator;
    [SerializeField] private GameObject _secondJumpIndicator;

    private void Update()
    {
        _firstJumpIndicator.SetActive(_player.FirstJump);
        _secondJumpIndicator.SetActive(_player.SecondJump);
    }
}
