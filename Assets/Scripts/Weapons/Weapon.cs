using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private Projectile _prefab;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _damage;
    [SerializeField] private float _shootDelay;

    private Coroutine _shootCoroutine;

    public abstract void Shoot();

    private IEnumerator ShootCoroutine()
    {
        while (isActiveAndEnabled)
        {
            Shoot();
            yield return new WaitForSeconds(_shootDelay);
        }
    }
}
