using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyTracker
{
    private readonly List<Enemy> _living = new();
    private readonly Dictionary<Enemy, Action> _handlers = new();

    public IReadOnlyList<Enemy> Living => _living;

    public int ActiveCount => _living.Count;

    public event Action<Enemy> EnemyDied;

    public event Action AllCleared;

    public bool Track(Enemy enemy)
    {
        if (enemy == null)
            return false;

        Health health = enemy.GetComponent<Health>();

        if (health == null)
        {
            Debug.LogError("[Waves] У спавнутого врага нет Health — враг не будет учтён.", enemy);
            return false;
        }

        if (_living.Contains(enemy))
            return false;

        if (_handlers.TryGetValue(enemy, out Action oldHandler))
            health.Died -= oldHandler;

        Action handler = () => HandleDied(enemy, health);
        health.Died += handler;

        _handlers[enemy] = handler;
        _living.Add(enemy);

        return true;
    }

    public bool IsTracking(Enemy enemy)
    {
        return _living.Contains(enemy);
    }

    public void Clear()
    {
        foreach (KeyValuePair<Enemy, Action> pair in _handlers)
        {
            if (pair.Key == null)
                continue;

            Health health = pair.Key.GetComponent<Health>();

            if (health != null)
                health.Died -= pair.Value;
        }

        _handlers.Clear();
        _living.Clear();
    }

    private void HandleDied(Enemy enemy, Health health)
    {
        if (_handlers.TryGetValue(enemy, out Action handler))
        {
            if (health != null)
                health.Died -= handler;

            _handlers.Remove(enemy);
        }

        _living.Remove(enemy);
        EnemyDied?.Invoke(enemy);

        if (_living.Count == 0)
            AllCleared?.Invoke();
    }
}
