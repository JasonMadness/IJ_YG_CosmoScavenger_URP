using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private PlayerRotator _playerRotator;

    private void OnEnable()
    {
        _playerMover.MoveDirectionChanged += _playerRotator.OnMoveDirectionChanged;
    }

    private void OnDisable()
    {
        _playerMover.MoveDirectionChanged -= _playerRotator.OnMoveDirectionChanged;
    }
}