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

    private Spawner spawner;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Asignación segura
    }

    void Update()
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
    }

    void ShooterBehavior()
    {
        if (player == null) return; // Verifica si el jugador sigue existiendo

        Vector3 direction = (player.position - firePoint.position).normalized;
        firePoint.rotation = Quaternion.LookRotation(direction);

        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<BulletEnemy>().damage = 10f; // Asigna el daño
        }
    }

    void MeleeBehavior()
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
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
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

    void Die()
    {
        Spawner.Instance.EnemyKilled();
        Destroy(gameObject);
    }
}
