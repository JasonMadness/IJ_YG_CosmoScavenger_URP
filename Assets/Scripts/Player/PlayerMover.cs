using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    private const float InputEpsilonSqr = 0.0001f;

    [SerializeField] private float _moveSpeed = 5f;

    private PlayerControls _playerControls;
    private Transform _transform;
    private Vector2 _moveInput;
    private Vector2 _moveDirection;
    private float _moveMagnitude;

    public event Action<Vector2> MoveDirectionChanged;

    public Vector2 MoveDirection => _moveDirection;

    public float MoveMagnitude => _moveMagnitude;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _transform = transform;
    }

    private void OnEnable()
    {
        _playerControls.Player.Move.performed += OnMove;
        _playerControls.Player.Move.canceled += OnMove;
        _playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Player.Disable();
        _playerControls.Player.Move.performed -= OnMove;
        _playerControls.Player.Move.canceled -= OnMove;

        _moveInput = Vector2.zero;
        _moveMagnitude = 0f;
        SetDirection(Vector2.zero);
    }

    private void OnDestroy()
    {
        _playerControls.Dispose();
    }

    public void IncreaseSpeed(float percent)
    {
        _moveSpeed *= 1f + percent;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.ClampMagnitude(context.ReadValue<Vector2>(), 1f);
    }

    private void Update()
    {
        Vector2 input = _moveInput.sqrMagnitude > InputEpsilonSqr ? _moveInput : Vector2.zero;
        Vector2 direction = input.sqrMagnitude > InputEpsilonSqr ? input.normalized : Vector2.zero;

        _moveMagnitude = input.magnitude;
        SetDirection(direction);

        Vector3 movement = new Vector3(input.x, 0f, input.y);
        _transform.Translate(movement * (_moveSpeed * Time.deltaTime), Space.World);
    }

    private void SetDirection(Vector2 direction)
    {
        if (_moveDirection == direction)
            return;

        _moveDirection = direction;
        MoveDirectionChanged?.Invoke(_moveDirection);
    }
}
