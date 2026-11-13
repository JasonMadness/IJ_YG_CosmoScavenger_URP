using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private Player _player;

    public Enemy Create()
    {
        Enemy enemy = _enemyPool.Get();
        enemy.Initialize(_player, _enemyPool);
        return enemy;
    }
}
