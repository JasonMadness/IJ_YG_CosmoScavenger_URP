using UnityEngine;

[System.Serializable]
public sealed class WaveEnemyData
{
    [SerializeField] private float _healthMultiplier = 1f;
    [SerializeField] private float _damageMultiplier = 1f;
    [SerializeField] private float _speedMultiplier = 1f;
    [SerializeField] private float _attackSpeedMultiplier = 1f;
    [SerializeField] private float _experienceMultiplier = 1f;

    public float HealthMultiplier => _healthMultiplier;

    public float DamageMultiplier => _damageMultiplier;

    public float SpeedMultiplier => _speedMultiplier;

    public float AttackSpeedMultiplier => _attackSpeedMultiplier;

    public float ExperienceMultiplier => _experienceMultiplier;

    public EnemyWaveScale ToScale()
    {
        return new EnemyWaveScale(
            _healthMultiplier,
            _damageMultiplier,
            _speedMultiplier,
            _attackSpeedMultiplier,
            _experienceMultiplier);
    }
}

[System.Serializable]
public sealed class WaveEnemyProgression
{
    private const int DefaultFinalWave = 6;

    [SerializeField] private WaveEnemyData[] _waves = new WaveEnemyData[DefaultFinalWave];

    [SerializeField] private float _bossHealthMultiplier = 5f;
    [SerializeField] private float _bossDamageMultiplier = 2f;
    [SerializeField] private float _bossSpeedMultiplier = 1.1f;
    [SerializeField] private float _bossExperienceMultiplier = 10f;

    public int FinalWave => _waves != null && _waves.Length > 0 ? _waves.Length : DefaultFinalWave;

    public EnemyWaveScale GetScale(int wave)
    {
        if (_waves == null || wave < 1 || wave > _waves.Length)
            return EnemyWaveScale.Neutral;

        WaveEnemyData data = _waves[wave - 1];

        return data == null ? EnemyWaveScale.Neutral : data.ToScale();
    }

    public EnemyWaveScale GetBossScale()
    {
        return new EnemyWaveScale(
            _bossHealthMultiplier,
            _bossDamageMultiplier,
            _bossSpeedMultiplier,
            1f,
            _bossExperienceMultiplier);
    }
}
