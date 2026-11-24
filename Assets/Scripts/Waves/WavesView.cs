using UnityEngine;
using UnityEngine.UI;

public class WavesView : MonoBehaviour
{
    [SerializeField] private Waves _waves;
    [SerializeField] private Text _waveText;

    private string _lastText;

    private void OnEnable()
    {
        _lastText = null;
    }

    private void Update()
    {
        if (_waves == null)
            return;

        string text = BuildText();

        if (text == _lastText)
            return;

        _lastText = text;

        if (_waveText != null)
            _waveText.text = text;

        Debug.Log("[Waves] " + text);
    }

    private string BuildText()
    {
        if (_waves.IsGameOver)
            return _waves.IsVictory ? "Игра окончена: победа!" : "Вы умерли";

        switch (_waves.Phase)
        {
            case WavePhase.Pause:
                return "Волна " + (_waves.CurrentWave + 1) + " через "
                       + Mathf.CeilToInt(_waves.PauseSecondsRemaining) + " сек";
            case WavePhase.Boss:
                return "Волна " + _waves.CurrentWave + ": БОСС";
            default:
                return "Волна " + _waves.CurrentWave;
        }
    }
}
