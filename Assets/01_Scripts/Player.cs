using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public static Player Instance;

    public bool finalPlayer;

    public float moveSpeed = 5f;            // Velocidad de movimiento
    public Transform firePoint;              // Empty para disparar
    public GameObject bulletPrefab;          // Prefab de la bala
    public float punchDamage = 15f;          // Daño del golpe
    private Rigidbody rb;
    public float maxHealth = 100f;           // Vida máxima del jugador
    public float currentHealth;              // Vida actual

    public float mouseSensitivity = 200f;     // Sensibilidad del mouse
    public Transform playerCamera;            // Referencia a la cámara
    private float xRotation = 0f;             // Rotación vertical de la cámara
    public float punchRange = 2f;             // Rango de golpe cuerpo a cuerpo
    public LayerMask enemyMask;               // Detectar enemigos
    private DeathMenu deathMenu;              // Referencia al menú de muerte

    // Audio
    public AudioClip moveClip;                // Sonido de movimiento
    public AudioClip fireClip;                // Sonido de disparo
    public AudioClip punchClip;               // Sonido de golpe
    public AudioClip damageClip;              // Sonido de recibir daño
    public AudioClip deathClip;               // Sonido de muerte

    private AudioSource audioSource;          // AudioSource para reproducir sonidos

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
        audioSource = GetComponent<AudioSource>(); // Inicializa el AudioSource
        currentHealth = maxHealth;                // Inicializa la vida completa
        Cursor.lockState = CursorLockMode.Locked;
        deathMenu = FindObjectOfType<DeathMenu>();  // Encuentra el menú de muerte
    }

    void Update()
    {
        if (currentHealth > 0) // Solo permite movimientos si el jugador está vivo
        {
            Movement();
            Fire();
            Punch();                               // Llamar a la función para golpear
            RotatePlayerWithMouse();
        }
    }

    void Movement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);

        // Reproduce sonido de movimiento si el jugador se mueve
        if ((moveX != 0 || moveZ != 0) && !audioSource.isPlaying)
        {
            PlaySound(moveClip);
        }
    }

    void Fire()
    {
        // Verifica si se presiona el botón de disparo
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Dispara una bala
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Bullet>().damage = 10f; // Ajusta el daño si es necesario

            // Reproduce sonido de disparo
            PlaySound(fireClip);

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
        if (Input.GetButtonDown("Fire2"))  // Asigna un botón para golpear (clic derecho)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, punchRange, enemyMask))
            {
                Debug.Log("Rayo golpeó: " + hit.transform.name); // Mensaje para verificar el golpe

                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(punchDamage);  // Aplica el daño
                    Debug.Log("Daño aplicado al enemigo: " + punchDamage); // Mensaje para verificar el daño

                    // Reproduce sonido de golpe
                    PlaySound(punchClip);
                }
                else
                {
                    // If it's not an enemy, check if it's the boss
                    BossDiablo bossDiablo = hit.transform.GetComponent<BossDiablo>();
                    if (bossDiablo != null)
                    {
                        bossDiablo.TakeDamage(punchDamage);  // Apply damage to the boss
                        Debug.Log("Damage applied to boss: " + punchDamage);  // Log message to verify damage

                        // Play punch sound
                        PlaySound(punchClip);
                    }
                    else
                    {
                        BossPapa bossPapa = hit.transform.GetComponent<BossPapa>();
                        if (bossPapa != null)
                        {
                            bossPapa.TakeDamage(punchDamage);  // Apply damage to the boss
                            Debug.Log("Damage applied to boss: " + punchDamage);  // Log message to verify damage

                            // Play punch sound
                            PlaySound(punchClip);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("No golpeó a ningún enemigo."); // Mensaje si no golpea
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Reproduce sonido de daño
        PlaySound(damageClip);

        if (currentHealth <= 0)
        {
            StartCoroutine(Die()); // Llama a la coroutine para morir
        }
    }

    IEnumerator Die()
    {
        // Reproduce sonido de muerte
        PlaySound(deathClip);

        yield return new WaitForSeconds(1f); // Espera un segundo para permitir que el sonido se reproduzca

        if (!finalPlayer)
        {
            Debug.Log("Jugador ha muerto.");
            deathMenu.ShowDeathMenu();  // Muestra el menú de muerte
            gameObject.SetActive(false);  // Desactiva al jugador cuando muere
        }
        else
        {
            SceneManager.LoadScene("Main Menu");
        }
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
            Debug.Log("Player se curó");
            // lifeBar.fillAmount = currentHealth / maxHealth;
        }
    }

    public void MoreDamage(float amount)
    {
        if (punchDamage < 50f)
        {
            punchDamage += amount;
            Debug.Log("Recibió más daño");
        }
    }

    // Función para reproducir sonidos
    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}