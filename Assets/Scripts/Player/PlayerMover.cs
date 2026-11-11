using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    private PlayerControls _playerControls;
    private Vector2 _moveInput;
    private Vector2 _moveDirection;

    public event Action<Vector2> MoveDirectionChanged;  

    public Vector2 MoveDirection => _moveDirection;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Player.Enable();
        
        _playerControls.Player.Move.performed += OnMove;
        _playerControls.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        _playerControls.Player.Disable();
        
        _playerControls.Player.Move.performed -= OnMove;
        _playerControls.Player.Move.canceled -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        if (_moveInput.sqrMagnitude < 0.0001f)
        {
            _moveDirection = Vector2.zero;
            MoveDirectionChanged?.Invoke(_moveDirection);
        }
    }

    private void Update()
    {
        Vector3 movement = new Vector3(_moveInput.x, 0f, _moveInput.y);

        if (movement.sqrMagnitude > 0.0001f)
        {
            Vector2 currentDirection = new Vector2(movement.x, movement.z).normalized;

            if (currentDirection != _moveDirection)
            {
                _moveDirection = currentDirection;
                MoveDirectionChanged?.Invoke(_moveDirection);
            }
        }

        transform.Translate(movement * (_moveSpeed * Time.deltaTime));
    }
}