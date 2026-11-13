using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Rigidbody _rigidbody;

    private ProjectilePool _pool;
    private bool _isReleased;

    public void Initialize(ProjectilePool pool)
    {
        _pool = pool;
        _isReleased = false;
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = transform.forward * _speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isReleased || _pool == null)
            return;

        _isReleased = true;
        _pool.Release(this);
    }
}
