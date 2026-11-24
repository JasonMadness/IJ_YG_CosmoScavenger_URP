using UnityEngine;

public class Waves : MonoBehaviour
{
    [Header("Баланс")]
    [SerializeField] private WaveBalance _waveBalance = new();

    [Header("Арена")]
    [SerializeField] private ArenaSettings _arenaSettings = new();

    [Header("Префабы")]
    [SerializeField] private GameObject _bossPrefab;

    [Header("Ссылки")]
    [SerializeField] private EnemyFactory _enemyFactory;
    [SerializeField] private Player _player;
    [SerializeField] private Health _playerHealth;

    [SerializeField] private int _currentWave = 1;

    private readonly GameSession _session = new();

    private WaveRules _rules;
    private WaveSpawner _spawner;
    private EnemyTracker _tracker;
    private StrayEnemyGuard _guard;

    private WavePhase _phase;
    private int _enemiesToSpawn;
    private float _lastSpawnTime;
    private float _pauseStartedAt;

    public int CurrentWave => _currentWave;

    public WavePhase Phase => _phase;

    public float PauseSecondsRemaining
    {
        get
        {
            if (_phase != WavePhase.Pause)
                return 0f;

            return Mathf.Max(0f, _waveBalance.PauseBetweenWaves - (Time.time - _pauseStartedAt));
        }
    }

    public bool IsGameOver => _session.IsFinished;

    public bool IsVictory => _session.IsVictory;

    private void Awake()
    {
        _rules = new WaveRules(_waveBalance.FirstWaveCount, _waveBalance.GrowthStep);
        ArenaSpawnPoints spawnPoints = new ArenaSpawnPoints(_arenaSettings.Bounds);
        ArenaGround ground = new ArenaGround(_arenaSettings.Bounds, _arenaSettings.FloorMask, _arenaSettings.GroundRayHeight);
        _spawner = new WaveSpawner(_enemyFactory, _bossPrefab, _player, spawnPoints, ground);
        _tracker = new EnemyTracker();
        _guard = new StrayEnemyGuard(
            _tracker,
            _arenaSettings.Bounds,
            _arenaSettings.GuardMargin,
            _arenaSettings.DeadTimeoutSeconds,
            _arenaSettings.ApproachEpsilon);

        _tracker.EnemyDied += _guard.Forget;
        _tracker.AllCleared += HandleAllEnemiesCleared;
        _session.Ended += HandleGameEnded;
    }

    private void OnEnable()
    {
        _session.BindPlayer(_playerHealth);
    }

    private void OnDisable()
    {
        _session.UnbindPlayer();
    }

    private void Start()
    {
        _currentWave = 1;
        _enemiesToSpawn = _rules.CountForWave(_currentWave);
        _phase = WavePhase.Spawning;
        _lastSpawnTime = Time.time - _waveBalance.SpawnInterval;
    }

    private void Update()
    {
        if (_session.IsFinished)
            return;

        switch (_phase)
        {
            case WavePhase.Spawning:
                TickSpawning();
                break;
            case WavePhase.WaitingForEnemies:
                TickWaitingForEnemies();
                break;
            case WavePhase.Pause:
                TickPause();
                break;
        }

        _guard.Tick(Time.time);
    }

    private void OnDestroy()
    {
        _session.UnbindPlayer();
        _session.Ended -= HandleGameEnded;

        if (_tracker != null)
        {
            _tracker.EnemyDied -= _guard.Forget;
            _tracker.AllCleared -= HandleAllEnemiesCleared;
            _tracker.Clear();
        }

        if (_guard != null)
            _guard.Clear();
    }

    private void TickSpawning()
    {
        if (_enemiesToSpawn > 0)
        {
            if (Time.time >= _lastSpawnTime + _waveBalance.SpawnInterval)
            {
                Enemy enemy = _spawner.SpawnRegular();

                if (enemy == null)
                {
                    _session.Lose();
                    return;
                }

                _tracker.Track(enemy);
                _enemiesToSpawn--;
                _lastSpawnTime = Time.time;
            }

            return;
        }

        _phase = WavePhase.WaitingForEnemies;
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

        _phase = WavePhase.Pause;
        _pauseStartedAt = Time.time;
    }

    private void TickPause()
    {
        if (PauseSecondsRemaining > 0f)
            return;

        _currentWave++;
        _enemiesToSpawn = _rules.CountForWave(_currentWave);
        _phase = WavePhase.Spawning;
    }

    private void SpawnBoss()
    {
        _phase = WavePhase.Boss;

        Enemy boss = _spawner.SpawnBoss();

        if (boss == null)
        {
            _session.Win();
            return;
        }

        _tracker.Track(boss);
    }

    private void HandleAllEnemiesCleared()
    {
        if (_phase != WavePhase.Boss)
            return;

        _session.Win();
    }

    private void HandleGameEnded(bool victory)
    {
        _phase = victory ? WavePhase.Victory : WavePhase.GameOver;
    }
}
