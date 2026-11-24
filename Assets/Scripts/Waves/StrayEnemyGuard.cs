using System.Collections.Generic;
using UnityEngine;

public sealed class StrayEnemyGuard
{
    private sealed class LostTrack
    {
        public float BestDistance = float.MaxValue;
        public float LastApproachTime;
    }

    private readonly EnemyTracker _tracker;
    private readonly Bounds _arenaBounds;
    private readonly float _margin;
    private readonly float _timeout;
    private readonly float _epsilon;
    private readonly Dictionary<Enemy, LostTrack> _tracks = new();
    private readonly List<Enemy> _expired = new();

    public StrayEnemyGuard(EnemyTracker tracker, Bounds arenaBounds, float margin, float timeout, float epsilon)
    {
        _tracker = tracker;
        _arenaBounds = arenaBounds;
        _margin = margin;
        _timeout = timeout;
        _epsilon = epsilon;
    }

    public void Tick(float now)
    {
        IReadOnlyList<Enemy> living = _tracker.Living;

        if (living.Count == 0)
            return;

        Vector3 center = _arenaBounds.center;
        Vector3 extents = _arenaBounds.extents + Vector3.one * _margin;

        _expired.Clear();

        for (int i = 0; i < living.Count; i++)
        {
            Enemy enemy = living[i];

            if (enemy == null)
                continue;

            float distance = DistanceToArena(enemy.transform.position, center, extents);

            if (distance <= 0f)
            {
                _tracks.Remove(enemy);
                continue;
            }

            if (!_tracks.TryGetValue(enemy, out LostTrack track))
            {
                _tracks[enemy] = new LostTrack
                {
                    BestDistance = distance,
                    LastApproachTime = now
                };
                continue;
            }

            if (distance < track.BestDistance - _epsilon)
            {
                track.BestDistance = distance;
                track.LastApproachTime = now;
                continue;
            }

            if (now - track.LastApproachTime >= _timeout)
                _expired.Add(enemy);
        }

        for (int i = 0; i < _expired.Count; i++)
        {
            Enemy enemy = _expired[i];

            if (enemy == null || !_tracker.IsTracking(enemy))
                continue;

            Debug.Log("[Waves] Враг вне арены и не приближается дольше " + _timeout
                      + " сек — считаю мёртвым.", enemy);

            Health health = enemy.GetComponent<Health>();

            if (health != null)
                health.TakeDamage(float.MaxValue);
            else
                enemy.gameObject.SetActive(false);
        }
    }

    public void Forget(Enemy enemy)
    {
        _tracks.Remove(enemy);
    }

    public void Clear()
    {
        _tracks.Clear();
        _expired.Clear();
    }

    public static float DistanceToArena(Vector3 position, Vector3 center, Vector3 extents)
    {
        float dx = Mathf.Abs(position.x - center.x) - extents.x;
        float dz = Mathf.Abs(position.z - center.z) - extents.z;

        if (dx <= 0f && dz <= 0f)
            return 0f;

        float ox = Mathf.Max(dx, 0f);
        float oz = Mathf.Max(dz, 0f);
        return Mathf.Sqrt(ox * ox + oz * oz);
    }
}
