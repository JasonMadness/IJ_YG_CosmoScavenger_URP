using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private PlayerControls playerControls;
    private Vector2 moveInput;

    private void Awake()
    {
        // Инициализируем наш сгенерированный класс управления
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
        
        // Подписываемся на событие движения
        playerControls.Player.Move.performed += OnMove;
        playerControls.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
        
        playerControls.Player.Move.performed -= OnMove;
        playerControls.Player.Move.canceled -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Считываем вектор (WASD)
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        // Двигаем капсулу в 3D-пространстве.
        // По X двигаем влево/вправо (A/D), по Z двигаем вперед/назад (W/S)
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}