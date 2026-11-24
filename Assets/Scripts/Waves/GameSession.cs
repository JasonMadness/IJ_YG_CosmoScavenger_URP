using System;

public sealed class GameSession
{
    private Health _playerHealth;

    public bool IsFinished { get; private set; }

    public bool IsVictory { get; private set; }

    public event Action<bool> Ended;

    public void BindPlayer(Health playerHealth)
    {
        UnbindPlayer();

        _playerHealth = playerHealth;

        if (_playerHealth != null)
            _playerHealth.Died += Lose;
    }

    public void UnbindPlayer()
    {
        if (_playerHealth != null)
            _playerHealth.Died -= Lose;

        _playerHealth = null;
    }

    public void Win()
    {
        Finish(true);
    }

    public void Lose()
    {
        Finish(false);
    }

    private void Finish(bool victory)
    {
        if (IsFinished)
            return;

        IsFinished = true;
        IsVictory = victory;
        Ended?.Invoke(victory);
    }
}
