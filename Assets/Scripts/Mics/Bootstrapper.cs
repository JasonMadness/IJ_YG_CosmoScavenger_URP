using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private Pause _pause;

    private void OnEnable()
    {
        if (_pause != null)
            _pause.PauseNeeded += _pause.OnPauseNeeded;
    }

    private void OnDisable()
    {
        if (_pause != null)
            _pause.PauseNeeded -= _pause.OnPauseNeeded;
    }
}
