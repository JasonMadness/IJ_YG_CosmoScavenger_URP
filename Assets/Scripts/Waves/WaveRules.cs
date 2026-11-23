public sealed class WaveRules
{
    public const int FinalWave = 6;

    private readonly int _firstWaveCount;
    private readonly int _growthStep;

    public WaveRules(int firstWaveCount, int growthStep)
    {
        _firstWaveCount = firstWaveCount;
        _growthStep = growthStep;
    }

    public int CountForWave(int wave)
    {
        return _firstWaveCount + _growthStep * (wave - 1);
    }
}
