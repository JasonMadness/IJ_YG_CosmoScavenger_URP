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
    }

    private void Update()
    {
        Vector3 movement = new Vector3(_moveInput.x, 0f, _moveInput.y);

        Vector2 currentDirection = new Vector2(movement.x, movement.z);

        if (currentDirection != _moveDirection)
        {
            _moveDirection = currentDirection;
            MoveDirectionChanged?.Invoke(_moveDirection);
        }

        transform.Translate(movement * (_moveSpeed * Time.deltaTime));
    }
}