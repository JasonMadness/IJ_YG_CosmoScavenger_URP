using System;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] private Player _target;
    [SerializeField] private Vector3 _offset;

    private void Start()
    {
        if (_target == null)
            throw new NullReferenceException(nameof(_target));
    }

    private void LateUpdate()
    {
        transform.position = _target.transform.position + _offset;
    }
}
