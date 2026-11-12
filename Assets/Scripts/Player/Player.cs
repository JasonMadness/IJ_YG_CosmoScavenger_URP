using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private PlayerRotator _playerRotator;
    [SerializeField] private PlayerShooter _playerShooter;
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private EnemyDetector _enemyDetector;

    private void OnEnable()
    {
        _playerMover.MoveDirectionChanged += _playerRotator.OnMoveDirectionChanged;
        _enemyDetector.ClosestEnemyDetected += _playerRotator.OnEnemyDetected;
        _enemyDetector.ClosestEnemyDetected += _playerShooter.OnEnemyDetected;
    }

    private void OnDisable()
    {
        _playerMover.MoveDirectionChanged -= _playerRotator.OnMoveDirectionChanged;
        _enemyDetector.ClosestEnemyDetected -= _playerRotator.OnEnemyDetected;
        _enemyDetector.ClosestEnemyDetected -= _playerShooter.OnEnemyDetected;
    }
}