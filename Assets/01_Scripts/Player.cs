using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public float moveSpeed = 5f;            // Velocidad de movimiento
    public Transform firePoint;              // Empty para disparar
    public GameObject bulletPrefab;          // Prefab de la bala
    public float punchDamage = 15f;          // Da�o del golpe
    private Rigidbody rb;
    public float maxHealth = 100f;           // Vida m�xima del jugador
    public float currentHealth;              // Vida actual

    public float mouseSensitivity = 200f;     // Sensibilidad del mouse
    public Transform playerCamera;            // Referencia a la c�mara

    private float xRotation = 0f;             // Rotaci�n vertical de la c�mara
    public float punchRange = 2f;             // Rango de golpe cuerpo a cuerpo
    public LayerMask enemyMask;               // Detectar enemigos
    private DeathMenu deathMenu;              // Referencia al men� de muerte

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;            // Inicializa la vida completa
        Cursor.lockState = CursorLockMode.Locked;
        deathMenu = FindObjectOfType<DeathMenu>();  // Encuentra el men� de muerte
    }

    void Update()
    {
        if (currentHealth > 0) // Solo permite movimientos si el jugador est� vivo
        {
            Movement();
            Fire();
            Punch();                               // Llamar a la funci�n para golpear
            RotatePlayerWithMouse();
        }
    }

    void Movement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);
    }

    void Fire()
    {
        // Verifica si se presiona el bot�n de disparo
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Dispara una bala
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Bullet>().damage = 10f; // Ajusta el da�o si es necesario
            Debug.Log("Disparando una bala"); // Log para verificar disparos
        }
    }

    void RotatePlayerWithMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void Punch()
    {
        if (Input.GetButtonDown("Fire2"))  // Asigna un bot�n para golpear (clic derecho)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, punchRange, enemyMask))
            {
                Debug.Log("Rayo golpe�: " + hit.transform.name); // Mensaje para verificar el golpe

                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(punchDamage);  // Aplica el da�o
                    Debug.Log("Da�o aplicado al enemigo: " + punchDamage); // Mensaje para verificar el da�o
                }
            }
            else
            {
                Debug.Log("No golpe� a ning�n enemigo."); // Mensaje si no golpea
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Jugador ha muerto.");
        deathMenu.ShowDeathMenu();  // Muestra el men� de muerte
        gameObject.SetActive(false);  // Desactiva al jugador cuando muere
    }

    public void HealPlayer(float amount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += amount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            Debug.Log("Player se curo");
            //lifeBar.fillAmount = currentHealth / maxHealth;
        }
    }

    public void MoreDamage(float amount)
    {
        if (punchDamage < 50f)
        {
            punchDamage += amount;
            Debug.Log("Recibio mas danio");
        }
    }
}
