using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    public GameObject bossPapaPrefab;
    public GameObject allyPrefab;
    public GameObject[] enemyPrefabs;  // Prefabs de los enemigos
    public Transform[] spawnPoints;    // Puntos donde aparecen los enemigos
    public float spawnRate = 3f;       // Intervalo de tiempo entre spawns
    private float nextSpawn = 0f;
    public int enemiesKilled = 0;       // Number of enemies killed
    public int enemiesToKillForBoss = 1; // Number of kills required to spawn the boss
    public bool bossSpawned = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Start ally spawn coroutine
        StartCoroutine(SpawnAllyRoutine());
    }

    void Update()
    {
        if (!bossSpawned)
        {
            if (enemiesKilled >= enemiesToKillForBoss)
            {
                SpawnBoss();
            }
            else if (Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnRate;
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);

        Instantiate(enemyPrefabs[randomEnemyIndex], spawnPoints[randomSpawnIndex].position, Quaternion.identity);
    }

    void SpawnBoss()
    {
        bossSpawned = true;
        GameObject boss = Instantiate(bossPapaPrefab, spawnPoints[0].position, Quaternion.identity); // Spawns boss at the first spawn point

        // Find the camera attached to the boss
        Camera bossCamera = boss.GetComponentInChildren<Camera>();

        if (bossCamera != null)
        {
            // Find the CameraSwitcher on the player and assign the boss camera
            CameraSwitcher cameraSwitcher = FindObjectOfType<CameraSwitcher>();
            if (cameraSwitcher != null)
            {
                cameraSwitcher.AssignBossCamera(bossCamera);  // Assign the boss camera
                cameraSwitcher.SwitchToBossCamera();          // Switch to the boss camera
            }
        }
    }

    public void EnemyKilled()
    {
        enemiesKilled++;
    }

    // Coroutine to spawn an ally every 60 seconds
    IEnumerator SpawnAllyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(60f);  // Wait for 1 minute

            // Spawn ally at a random spawn point
            int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
            Instantiate(allyPrefab, spawnPoints[randomSpawnIndex].position, Quaternion.identity);
        }
    }
}