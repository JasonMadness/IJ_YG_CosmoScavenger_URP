using UnityEngine;

public sealed class WaveSpawner
{
    private readonly EnemyFactory _enemyFactory;
    private readonly GameObject _bossPrefab;
    private readonly Player _player;
    private readonly ArenaSpawnPoints _spawnPoints;

    public WaveSpawner(EnemyFactory enemyFactory, GameObject bossPrefab, Player player, ArenaSpawnPoints spawnPoints)
    {
        _enemyFactory = enemyFactory;
        _bossPrefab = bossPrefab;
        _player = player;
        _spawnPoints = spawnPoints;
    }

    public Enemy SpawnRegular()
    {
        if (_enemyFactory == null)
        {
            Debug.LogError("[Waves] EnemyFactory не назначен в Inspector.");
            return null;
        }

        Enemy enemy = _enemyFactory.Create();

        if (enemy == null)
            return null;

        enemy.Spawn(_spawnPoints.Next());

        return enemy;
    }

    public Enemy SpawnBoss()
    {
        if (_bossPrefab == null)
        {
            Debug.LogWarning("[Waves] Boss prefab не назначен — засчитываю победу без боя.");
            return null;
        }

        GameObject bossObject = Object.Instantiate(_bossPrefab, _spawnPoints.Next(), Quaternion.identity);
        Enemy boss = bossObject.GetComponent<Enemy>();

        if (boss == null)
        {
            Debug.LogError("[Waves] На bossPrefab нет компонента Enemy — победа недостижима.");
            Object.Destroy(bossObject);
            return null;
        }

        if (_player == null)
        {
            Debug.LogError("[Waves] Player не назначен в Inspector — босс не найдёт цель.");
            Object.Destroy(bossObject);
            return null;
        }

        boss.Initialize(_player, null);
        boss.Spawn(bossObject.transform.position);

        return boss;
    }
}
