using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private ProjectileFactory _factory;

    private readonly Queue<Projectile> _projectiles = new();

    public Projectile Get()
    {
        Projectile projectile = _projectiles.Count == 0
            ? _factory.Create()
            : _projectiles.Dequeue();

        projectile.Initialize(this);
        projectile.gameObject.SetActive(true);
        return projectile;
    }

    public void Release(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
        _projectiles.Enqueue(projectile);
    }
}
