using UnityEngine;
using UnityEngine.UI;

public class WavesView : MonoBehaviour
{
    [SerializeField] private Waves _waves;
    [SerializeField] private Text _waveText;

    private void OnEnable()
    {
        if (_waves == null)
            return;

        _waves.WaveStarted += ShowWave;
        _waves.PauseTick += ShowCountdown;
        _waves.BossAppeared += ShowBoss;
        _waves.GameEnded += ShowGameEnd;

        if (!_waves.IsGameOver)
            ShowWave(_waves.CurrentWave);
    }

    private void OnDisable()
    {
        if (_waves == null)
            return;

        _waves.WaveStarted -= ShowWave;
        _waves.PauseTick -= ShowCountdown;
        _waves.BossAppeared -= ShowBoss;
        _waves.GameEnded -= ShowGameEnd;
    }

    private void ShowWave(int wave)
    {
        SetText("Волна " + wave);
    }

    private void ShowCountdown(int nextWave, int seconds)
    {
        SetText("Волна " + nextWave + " через " + seconds + " сек");
    }

    private void ShowBoss(int wave)
    {
        SetText("Волна " + wave + ": БОСС");
    }

    private void ShowGameEnd(bool victory)
    {
        SetText(victory ? "Игра окончена: победа!" : "Вы умерли");
    }

    private void SetText(string text)
    {
        if (_waveText != null)
            _waveText.text = text;

        Debug.Log("[Waves] " + text);
    }
}
