using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private PlayerRotator _playerRotator;
    [SerializeField] private PlayerShooter _playerShooter;
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private Health _health;
    [SerializeField] private Experience _experience;

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

    public void GainExperience(int amount)
    {
        _experience.Add(amount);
    }

    public void IncreaseDamage(float percent)
    {
        _playerShooter.IncreaseDamage(percent);
    }

    public void IncreaseMoveSpeed(float percent)
    {
        _playerMover.IncreaseSpeed(percent);
    }

    public void IncreaseArmor(float amount)
    {
        _health.AddDamageResistance(amount);
    }
}
