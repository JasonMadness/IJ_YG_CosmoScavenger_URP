using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Slider _healthSlider;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        Refresh();
    }

    private void OnEnable()
    {
        _health.HealthChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        _health.HealthChanged -= Refresh;
    }

    private void LateUpdate()
    {
        if (_camera == null)
            return;

        transform.rotation = _camera.transform.rotation;
    }

    private void Refresh()
    {
        _healthSlider.value = _health.CurrentHealth / _health.MaxHealth;
    }
}
