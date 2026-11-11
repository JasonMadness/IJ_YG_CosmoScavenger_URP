using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private PlayerRotator _playerRotator;

    private void Start()
    {
        if (_playerMover == null)
            throw new System.NullReferenceException("PlayerMover reference is not assigned in the Player script.");

        if (_playerRotator == null)
            throw new System.NullReferenceException("PlayerRotator reference is not assigned in the Player script.");
    }

    private void OnEnable()
    {
        _playerMover.MoveDirectionChanged += _playerRotator.OnMoveDirectionChanged;
    }

    private void OnDisable()
    {
        _playerMover.MoveDirectionChanged -= _playerRotator.OnMoveDirectionChanged;
    }
}
