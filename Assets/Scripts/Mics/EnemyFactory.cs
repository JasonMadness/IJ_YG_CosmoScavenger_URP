using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Player _player;

    public Enemy Create()
    {
        Enemy enemy = Instantiate(_enemyPrefab);
        enemy.Initialize(_player);
        return enemy;
    }
}
