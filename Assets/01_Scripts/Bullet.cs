using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f; // Daño de la bala
    public float speed = 20f;   // Velocidad de la bala
    private float lifetime = 3f; // Tiempo de vida de la bala

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destruye la bala después de 3 segundos
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // Mueve la bala hacia adelante
    }

    // Detectar colisiones
    private void OnTriggerEnter(Collider hitInfo)
    {
        // Verifica si la bala golpea un enemigo
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Aplica daño al enemigo
            Debug.Log("Enemigo golpeado, daño aplicado: " + damage); // Log para confirmar el daño
            Destroy(gameObject); // Destruye la bala tras golpear al enemigo
        }

        // Verifica si la bala golpea un bossDiablo
        BossDiablo bossDiablo = hitInfo.GetComponent<BossDiablo>();
        if (bossDiablo != null)
        {
            bossDiablo.TakeDamage(damage); // Aplica daño al enemigo
            Debug.Log("Boss golpeado, daño aplicado: " + damage); // Log para confirmar el daño
            Destroy(gameObject); // Destruye la bala tras golpear al enemigo
        }

        // Verifica si la bala golpea un bossPapa
        BossPapa bossPapa = hitInfo.GetComponent<BossPapa>();
        if (bossPapa != null)
        {
            bossPapa.TakeDamage(damage); // Aplica daño al enemigo
            Debug.Log("Boss golpeado, daño aplicado: " + damage); // Log para confirmar el daño
            Destroy(gameObject); // Destruye la bala tras golpear al enemigo
        }
    }
}