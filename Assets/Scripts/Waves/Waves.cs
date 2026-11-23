using System;
using UnityEngine;

public class Waves : MonoBehaviour
{
    private enum Phase
    {
        Spawning,
        WaitingForEnemies,
        Pause,
        Boss,
        Victory,
        GameOver
    }

    [Header("Прогрессия")]
    [SerializeField] private int _firstWaveCount = 10;
    [SerializeField] private int _growthStep = 5;

    [Header("Тайминги")]
    [SerializeField] private float _spawnInterval = 0.8f;
    [SerializeField] private float _pauseBetweenWaves = 5f;
    [SerializeField] private float _deadTimeoutSeconds = 30f;

    [Header("Префабы")]
    [SerializeField] private GameObject _bossPrefab;

    [Header("Арена")]
    [SerializeField] private Bounds _arenaBounds = new Bounds(Vector3.zero, new Vector3(100f, 1f, 100f));
    [SerializeField] private float _guardMargin = 12f;
    [SerializeField] private float _approachEpsilon = 0.5f;

    [Header("Ссылки")]
    [SerializeField] private EnemyFactory _enemyFactory;
    [SerializeField] private Player _player;
    [SerializeField] private Health _playerHealth;

    [SerializeField] private int _currentWave = 1;

    private readonly GameSession _session = new();

    private WaveRules _rules;
    private ArenaSpawnPoints _spawnPoints;
    private EnemyTracker _tracker;
    private StrayEnemyGuard _guard;

    private Phase _phase;
    private int _enemiesToSpawn;
    private float _lastSpawnTime;
    private float _pauseStartedAt;

    public int CurrentWave => _currentWave;

    public bool IsGameOver => _session.IsFinished;

    public bool IsVictory => _session.IsVictory;

    public event Action<int> WaveStarted;

    public event Action<int, int> PauseTick;

    public event Action<int> BossAppeared;

    public event Action<bool> GameEnded;

    private void Awake()
    {
        _rules = new WaveRules(_firstWaveCount, _growthStep);
        _spawnPoints = new ArenaSpawnPoints(_arenaBounds);
        _tracker = new EnemyTracker();
        _guard = new StrayEnemyGuard(_tracker, _arenaBounds, _guardMargin, _deadTimeoutSeconds, _approachEpsilon);

        _tracker.EnemyDied += _guard.Forget;
        _tracker.AllCleared += HandleAllEnemiesCleared;
        _session.Ended += HandleGameEnded;
    }

    private void OnEnable()
    {
        if (_playerHealth != null)
            _playerHealth.Died += HandlePlayerDied;
    }

    private void OnDisable()
    {
        if (_playerHealth != null)
            _playerHealth.Died -= HandlePlayerDied;
    }

    private void Start()
    {
        _currentWave = 1;
        _enemiesToSpawn = _rules.CountForWave(_currentWave);
        _phase = Phase.Spawning;
        _lastSpawnTime = Time.time - _spawnInterval;

        WaveStarted?.Invoke(_currentWave);
    }

    private void Update()
    {
        if (_session.IsFinished)
            return;

        switch (_phase)
        {
            case Phase.Spawning:
                TickSpawning();
                break;
            case Phase.WaitingForEnemies:
                TickWaitingForEnemies();
                break;
            case Phase.Pause:
                TickPause();
                break;
        }

        _guard.Tick(Time.time);
    }

    private void OnDestroy()
    {
        if (_tracker != null)
        {
            _tracker.EnemyDied -= _guard.Forget;
            _tracker.AllCleared -= HandleAllEnemiesCleared;
            _tracker.Clear();
        }

        if (_guard != null)
            _guard.Clear();

        _session.Ended -= HandleGameEnded;
    }

    private void TickSpawning()
    {
        if (_enemiesToSpawn > 0)
        {
            if (Time.time >= _lastSpawnTime + _spawnInterval)
            {
                SpawnRegularEnemy();
                _enemiesToSpawn--;
                _lastSpawnTime = Time.time;
            }

            return;
        }

        _phase = Phase.WaitingForEnemies;
    }

    private void TickWaitingForEnemies()
    {
        if (_tracker.ActiveCount > 0)
            return;

        if (_currentWave >= WaveRules.FinalWave)
        {
            SpawnBoss();
            return;
        }

        _phase = Phase.Pause;
        _pauseStartedAt = Time.time;
    }

    private void TickPause()
    {
        float remaining = _pauseBetweenWaves - (Time.time - _pauseStartedAt);

        if (remaining > 0f)
        {
            PauseTick?.Invoke(_currentWave + 1, Mathf.CeilToInt(remaining));
            return;
        }

        _currentWave++;
        _enemiesToSpawn = _rules.CountForWave(_currentWave);
        _phase = Phase.Spawning;
        WaveStarted?.Invoke(_currentWave);
    }

    private void SpawnRegularEnemy()
    {
        if (_enemyFactory == null)
        {
            Debug.LogError("[Waves] EnemyFactory не назначен в Inspector.", this);
            _session.Lose();
            return;
        }

        Enemy enemy = _enemyFactory.Create();
        enemy.Spawn(_spawnPoints.Next());
        _tracker.Track(enemy);
    }

    private void SpawnBoss()
    {
        _phase = Phase.Boss;

        if (_bossPrefab == null)
        {
            Debug.LogWarning("[Waves] Boss prefab не назначен — засчитываю победу без боя.", this);
            _session.Win();
            return;
        }

        GameObject bossObject = Instantiate(_bossPrefab, _spawnPoints.Next(), Quaternion.identity);
        Enemy boss = bossObject.GetComponent<Enemy>();

        if (boss == null)
        {
            Debug.LogError("[Waves] На bossPrefab нет компонента Enemy — победа недостижима.", this);
            Destroy(bossObject);
            _session.Win();
            return;
        }

        if (_player == null)
        {
            Debug.LogError("[Waves] Player не назначен в Inspector — босс не найдёт цель.", this);
            Destroy(bossObject);
            _session.Win();
            return;
        }

        boss.Initialize(_player, null);
        boss.Spawn(bossObject.transform.position);
        _tracker.Track(boss);
        BossAppeared?.Invoke(_currentWave);
    }

    private void HandleAllEnemiesCleared()
    {
        if (_phase != Phase.Boss)
            return;

        _session.Win();
    }

    private void HandlePlayerDied()
    {
        _session.Lose();
    }

    private void HandleGameEnded(bool victory)
    {
        _phase = victory ? Phase.Victory : Phase.GameOver;
        GameEnded?.Invoke(victory);
    }
}
