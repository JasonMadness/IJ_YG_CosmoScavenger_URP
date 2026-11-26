using System.Collections.Generic;
using UnityEngine;

public sealed class RadarView : MonoBehaviour
{
    private const float DirectionEpsilonSqr = 0.0001f;

    [SerializeField] private Waves _waves;
    [SerializeField] private Transform _playerModel;
    [SerializeField] private RadarArrowPool _arrowPool;
    [SerializeField] private float _radius = 62f;
    [SerializeField] private float _maxRange = 40f;
    [SerializeField] private float _minRange = 6f;

    private readonly List<RadarArrow> _shown = new();

    private void LateUpdate()
    {
        if (_waves == null || _playerModel == null || _arrowPool == null)
            return;

        if (_waves.IsGameOver)
        {
            ReleaseFrom(0);
            return;
        }

        IReadOnlyList<Enemy> living = _waves.LivingEnemies;
        int used = 0;

        for (int i = 0; i < living.Count; i++)
        {
            Enemy enemy = living[i];

            if (enemy == null)
                continue;

            Vector3 toEnemy = enemy.transform.position - _playerModel.position;
            toEnemy.y = 0f;

            float distance = toEnemy.magnitude;

            if (distance < Mathf.Epsilon)
                continue;

            Vector2 direction = GetRadarDirection(toEnemy / distance);
            float radial = Mathf.Clamp01(distance / _maxRange);

            RadarArrow arrow = TakeArrow(used);
            arrow.Show(direction, Mathf.Lerp(_minRange, _radius, radial));
            used++;
        }

        ReleaseFrom(used);
    }

    private RadarArrow TakeArrow(int index)
    {
        if (index < _shown.Count)
            return _shown[index];

        RadarArrow arrow = _arrowPool.Get();
        _shown.Add(arrow);

        return arrow;
    }

    private void ReleaseFrom(int index)
    {
        for (int i = _shown.Count - 1; i >= index; i--)
        {
            _arrowPool.Release(_shown[i]);
            _shown.RemoveAt(i);
        }
    }

    private Vector2 GetRadarDirection(Vector3 toEnemyNormalized)
    {
        Vector3 forward = _playerModel.forward;
        forward.y = 0f;

        if (forward.sqrMagnitude < DirectionEpsilonSqr)
            forward = Vector3.forward;
        else
            forward.Normalize();

        Vector3 right = Vector3.Cross(Vector3.up, forward);

        return new Vector2(
            Vector3.Dot(toEnemyNormalized, right),
            Vector3.Dot(toEnemyNormalized, forward));
    }
}
