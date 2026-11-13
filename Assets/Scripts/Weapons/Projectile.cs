using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Rigidbody _rigidbody;

    private float _damage;
    private ProjectilePool _pool;
    private bool _isReleased;

    public void Initialize(ProjectilePool pool)
    {
        _pool = pool;
        _isReleased = false;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = transform.forward * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(_damage);
        }

        if (_pool == null)
            Destroy(gameObject);

        if (_isReleased)
            return;

        _isReleased = true;
        _pool.Release(this);
    }
}
