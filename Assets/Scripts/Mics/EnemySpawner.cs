using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _arenaHalfSize = 50f;
    [SerializeField] private float _spawnOffset = 5f;
    [SerializeField] private EnemyFactory _enemyFactory;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, _spawnInterval);
    }

    private void SpawnEnemy()
    {
        Enemy enemy = _enemyFactory.Create();
        enemy.transform.position = GetSpawnPosition();
    }

    private Vector3 GetSpawnPosition()
    {
        float outsideDistance = _arenaHalfSize + _spawnOffset;
        float alongAxis = Random.Range(-_arenaHalfSize, _arenaHalfSize);
        int side = Random.Range(0, 4);

        return side switch
        {
            0 => new Vector3(outsideDistance, 0f, alongAxis),
            1 => new Vector3(-outsideDistance, 0f, alongAxis),
            2 => new Vector3(alongAxis, 0f, outsideDistance),
            _ => new Vector3(alongAxis, 0f, -outsideDistance)
        };
    }
}
