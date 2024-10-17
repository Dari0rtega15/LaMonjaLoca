using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { Shooter, Melee }
    public EnemyType enemyType;

    public float health = 50f;
    public float moveSpeed = 3f;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float nextFire = 0f;

    public float meleeDamage = 10f;
    public float attackRange = 2f;
    private Transform player;
    public GameObject[] itemsPrefabs;
    public Transform dropPoint;

    public Animator anim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Asignación segura
    }

    private void Update()
    {
        if (player == null) return; // Salir si el jugador ha sido destruido

        if (enemyType == EnemyType.Shooter)
        {
            ShooterBehavior();
        }
        else if (enemyType == EnemyType.Melee)
        {
            MeleeBehavior();
        }

        // Update the animation based on the state
        UpdateAnimation();
    }

    private void ShooterBehavior()
    {
        if (player == null) return; // Verifica si el jugador sigue existiendo

        Vector3 direction = (player.position - firePoint.position).normalized;
        firePoint.rotation = Quaternion.LookRotation(direction);

        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<BulletEnemy>().damage = 10f; // Asigna el daño
            anim.SetTrigger("Attack"); // Trigger the attack animation
        }
    }

    private void MeleeBehavior()
    {
        if (player == null) return; // Verifica si el jugador sigue existiendo

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(meleeDamage);
                Debug.Log("Atacando al jugador cuerpo a cuerpo!");
                anim.SetTrigger("Attack"); // Trigger the attack animation
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            anim.SetBool("Walk", true); // Set walking animation
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Daño al enemigo: " + damage);
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        anim.SetTrigger("Die"); // Trigger the die animation
        Spawner.Instance.EnemyKilled();
        DropItem();
        Destroy(gameObject, 2f); // Delay destruction to allow the death animation to play
    }

    private void DropItem()
    {
        float dropChance = 0.3f;  // 30% chance

        // Check if the random value is below the drop chance
        if (Random.Range(0f, 1f) <= dropChance)
        {
            if (itemsPrefabs.Length > 0 && dropPoint != null)
            {
                // Randomly select an item from the list
                int randomItemIndex = Random.Range(0, itemsPrefabs.Length);
                GameObject selectedItem = itemsPrefabs[randomItemIndex];

                // Drop the randomly selected item at the drop point
                Instantiate(selectedItem, dropPoint.position, Quaternion.identity);
            }
        }
    }

    private void UpdateAnimation()
    {
        if (health <= 0)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (enemyType == EnemyType.Melee && distanceToPlayer > attackRange)
        {
            anim.SetBool("Walk", true); // Set walking animation
            anim.SetBool("Idle", false);   // Unset idle animation
        }
        else
        {
            anim.SetBool("Walk", false); // Unset walking animation
            anim.SetBool("Idle", true);     // Set idle animation
        }
    }
}
