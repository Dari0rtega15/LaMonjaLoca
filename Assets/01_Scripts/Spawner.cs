using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // Prefabs de los enemigos
    public Transform[] spawnPoints;    // Puntos donde aparecen los enemigos
    public float spawnRate = 3f;       // Intervalo de tiempo entre spawns
    private float nextSpawn = 0f;

    void Update()
    {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);

        Instantiate(enemyPrefabs[randomEnemyIndex], spawnPoints[randomSpawnIndex].position, Quaternion.identity);
    }
}