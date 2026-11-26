using UnityEngine;

public sealed class WaveSpawner
{
    private const float GroundEpsilon = 0.01f;

    private readonly EnemyFactory _enemyFactory;
    private readonly GameObject _bossPrefab;
    private readonly Player _player;
    private readonly ArenaSpawnPoints _spawnPoints;
    private readonly ArenaGround _ground;

    public WaveSpawner(EnemyFactory enemyFactory, GameObject bossPrefab, Player player, ArenaSpawnPoints spawnPoints, ArenaGround ground)
    {
        _enemyFactory = enemyFactory;
        _bossPrefab = bossPrefab;
        _player = player;
        _spawnPoints = spawnPoints;
        _ground = ground;
    }

    public Enemy SpawnRegular(EnemyWaveScale scale)
    {
        if (_enemyFactory == null)
        {
            Debug.LogError("[Waves] EnemyFactory не назначен в Inspector.");
            return null;
        }

        Enemy enemy = _enemyFactory.Create(scale);

        if (enemy == null)
            return null;

        enemy.Spawn(GetGroundedPosition(enemy, _spawnPoints.Next()));

        return enemy;
    }

    public Enemy SpawnBoss(EnemyWaveScale scale)
    {
        if (_bossPrefab == null)
        {
            Debug.LogWarning("[Waves] Boss prefab не назначен — засчитываю победу без боя.");
            return null;
        }

        Vector3 spawnPoint = _spawnPoints.Next();
        GameObject bossObject = Object.Instantiate(_bossPrefab, spawnPoint, Quaternion.identity);
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

        boss.Initialize(_player, null, scale);
        boss.Spawn(GetGroundedPosition(boss, spawnPoint));

        return boss;
    }

    private Vector3 GetGroundedPosition(Enemy enemy, Vector3 spawnPoint)
    {
        float floorY = _ground.GetFloorY(spawnPoint);

        Physics.SyncTransforms();

        float pivotToFeet = enemy.GetPivotToFeet();

        return new Vector3(spawnPoint.x, floorY + pivotToFeet + GroundEpsilon, spawnPoint.z);
    }
}
