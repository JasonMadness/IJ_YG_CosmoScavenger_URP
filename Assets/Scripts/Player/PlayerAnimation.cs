using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private readonly int SpeedHash = Animator.StringToHash("Speed");

    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerMover _mover;
    [SerializeField] private float _dampTime = 0.1f;

    private void LateUpdate()
    {
        _animator.SetFloat(SpeedHash, _mover.MoveMagnitude, _dampTime, Time.deltaTime);
    }
}