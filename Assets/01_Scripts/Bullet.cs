using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f; // Da�o de la bala
    public float speed = 20f;   // Velocidad de la bala
    private float lifetime = 3f; // Tiempo de vida de la bala

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destruye la bala despu�s de 3 segundos
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
            enemy.TakeDamage(damage); // Aplica da�o al enemigo
            Debug.Log("Enemigo golpeado, da�o aplicado: " + damage); // Log para confirmar el da�o
            Destroy(gameObject); // Destruye la bala tras golpear al enemigo
        }
    }
}