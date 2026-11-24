using System;
using UnityEngine;

[Serializable]
public sealed class ArenaSettings
{
    [SerializeField] private Bounds _bounds = new Bounds(Vector3.zero, new Vector3(100f, 1f, 100f));
    [SerializeField] private float _guardMargin = 12f;
    [SerializeField] private float _approachEpsilon = 0.5f;
    [SerializeField] private float _deadTimeoutSeconds = 30f;

    public Bounds Bounds => _bounds;

    public float GuardMargin => _guardMargin;

    public float ApproachEpsilon => _approachEpsilon;

    public float DeadTimeoutSeconds => _deadTimeoutSeconds;
}
