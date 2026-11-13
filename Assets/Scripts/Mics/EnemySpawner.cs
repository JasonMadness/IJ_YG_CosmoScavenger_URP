using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _boundary = 55f;

    public void SpawnEnemy(EnemyPool enemyPool)
    {
        System.Random random = new System.Random();
        bool x = Convert.ToBoolean(random.Next(0, 2));
        bool z = Convert.ToBoolean(random.Next(0, 2));

        Vector3 spawnPosition = new Vector3(x ? _boundary : -_boundary, 0.0f, z ? _boundary : -_boundary);
        Enemy enemy = enemyPool.Get();
        enemy.transform.position = spawnPosition;
    }
}
