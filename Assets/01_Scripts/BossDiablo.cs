using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDiablo : MonoBehaviour
{
    public Transform player;         // Player's transform
    public float moveSpeed = 3f;     // Boss movement speed
    public float tackleRange = 2f;   // Range at which the boss will tackle
    public float shootCooldown = 2f; // Time between projectile shots
    public GameObject projectilePrefab; // Prefab for the boss's projectile
    public Transform shootPoint;     // Where the projectiles are spawned from
    public float projectileSpeed = 10f; // Speed of the projectile
    public int tackleDamage = 20;    // Damage dealt by tackle

    private float shootTimer = 0f;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        FocusOnPlayer();

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > tackleRange)
        {
            RunToPlayer();
        }
        else
        {
            TacklePlayer(); // Tackle the player if in range
        }

        if (shootTimer <= 0f)
        {
            ShootAtPlayer();
            shootTimer = shootCooldown;
        }
        else
        {
            shootTimer -= Time.deltaTime;
        }
    }

    void FocusOnPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0; // Keeps the boss from tilting up/down
        Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
    }

    void RunToPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void TacklePlayer()
    {
        Debug.Log("Boss tacleo al player");
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.TakeDamage(tackleDamage);
        }
    }

    void ShootAtPlayer()
    {
        Debug.Log("Boss dispar proyectil!");

        // Create a projectile and shoot towards the player
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Vector3 directionToPlayer = (player.position - shootPoint.position).normalized;
        projectile.GetComponent<Rigidbody>().velocity = directionToPlayer * projectileSpeed;
    }
}
