using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    [SerializeField] private Projectile _projectilePrefab;

    public Projectile Create()
    {
        return Instantiate(_projectilePrefab, transform);
    }
}
