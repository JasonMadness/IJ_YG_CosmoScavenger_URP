using UnityEngine;

public sealed class ArenaGround
{
    private readonly Bounds _arenaBounds;
    private readonly LayerMask _floorMask;
    private readonly float _rayHeight;

    private bool _hasWarned;

    public ArenaGround(Bounds arenaBounds, LayerMask floorMask, float rayHeight)
    {
        _arenaBounds = arenaBounds;
        _floorMask = floorMask;
        _rayHeight = rayHeight;
    }

    public float GetFloorY(Vector3 position)
    {
        Vector3 origin = new Vector3(position.x, _arenaBounds.max.y + _rayHeight, position.z);
        float distance = _arenaBounds.size.y + _rayHeight * 2f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, distance, _floorMask, QueryTriggerInteraction.Ignore))
            return hit.point.y;

        if (_hasWarned == false)
        {
            _hasWarned = true;
            Debug.LogWarning("[Waves] Пол не найден под точкой спавна " + position + " — использую Y арены.");
        }

        return _arenaBounds.center.y;
    }
}
