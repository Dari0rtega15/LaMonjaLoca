using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public float damage = 10f;     // Daño de la bala
    public float speed = 20f;      // Velocidad de la bala
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;  // Desactiva la gravedad
        rb.velocity = transform.forward * speed;  // Mueve la bala hacia adelante

        // Destruir la bala después de 3 segundos
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter(Collider hitInfo)
    {
        Player player = hitInfo.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage);
            Debug.Log("Bala del enemigo golpeó al jugador.");
            Destroy(gameObject); // Destruir la bala tras colisionar
        }
    }
}