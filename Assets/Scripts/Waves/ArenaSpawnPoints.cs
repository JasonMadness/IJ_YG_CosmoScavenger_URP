using UnityEngine;

public sealed class ArenaSpawnPoints
{
    private const float OutsideOffset = 5f;

    private readonly Bounds _arenaBounds;

    public ArenaSpawnPoints(Bounds arenaBounds)
    {
        _arenaBounds = arenaBounds;
    }

    public Vector3 Next()
    {
        Vector3 center = _arenaBounds.center;
        Vector3 extents = _arenaBounds.extents;
        float along = Random.Range(center.x - extents.x, center.x + extents.x);

        return Random.Range(0, 4) switch
        {
            0 => new Vector3(center.x + extents.x + OutsideOffset, center.y, along),
            1 => new Vector3(center.x - extents.x - OutsideOffset, center.y, along),
            2 => new Vector3(along, center.y, center.z + extents.z + OutsideOffset),
            _ => new Vector3(along, center.y, center.z - extents.z - OutsideOffset)
        };
    }
}
