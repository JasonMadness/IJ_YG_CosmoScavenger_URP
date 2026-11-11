using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _maxHealth = 3f;
    
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        if (_target != null)
        {
            Vector3 direction = (_target.transform.position - transform.position).normalized;
            transform.position += direction * _moveSpeed * Time.deltaTime;
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}