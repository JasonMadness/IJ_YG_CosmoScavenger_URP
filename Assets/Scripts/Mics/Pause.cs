using UnityEngine;

public class Pause : MonoBehaviour
{
    private const float PausedTimeScale = 0f;
    private const float NormalTimeScale = 1f;

    private void OnEnable()
    {
        Time.timeScale = NormalTimeScale;
    }

    public void OnPauseNeeded()
    {
        Time.timeScale = PausedTimeScale;
    }

    public void OnResumeNeeded()
    {
        Time.timeScale = NormalTimeScale;
    }
}
