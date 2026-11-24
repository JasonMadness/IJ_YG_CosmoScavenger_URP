using System;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public event Action PauseNeeded;

    public void RequestPause()
    {
        PauseNeeded?.Invoke();
    }

    public void OnPauseNeeded()
    {
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }
}
