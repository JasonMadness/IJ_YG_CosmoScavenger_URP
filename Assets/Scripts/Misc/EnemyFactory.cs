using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private Player _player;

    public Enemy Create(EnemyWaveScale scale)
    {
        Enemy enemy = _enemyPool.Get();
        enemy.Initialize(_player, _enemyPool, scale);
        return enemy;
    }
}
