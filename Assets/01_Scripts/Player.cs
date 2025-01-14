using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public bool finalPlayer;

    public float moveSpeed = 5f;            // Velocidad de movimiento
    public Transform firePoint;              // Empty para disparar
    public GameObject bulletPrefab;          // Prefab de la bala
    public float punchDamage = 15f;          // Da�o del golpe
    private Rigidbody rb;
    public float maxHealth = 20f;           // Vida m�xima del jugador
    public float currentHealth;              // Vida actual

    public float mouseSensitivity = 200f;     // Sensibilidad del mouse
    public Transform playerCamera;            // Referencia a la c�mara
    private float xRotation = 0f;             // Rotaci�n vertical de la c�mara
    public float punchRange = 2f;             // Rango de golpe cuerpo a cuerpo
    public LayerMask enemyMask;               // Detectar enemigos
    private DeathMenu deathMenu;              // Referencia al men� de muerte

    // Audio
    public AudioClip moveClip;                // Sonido de movimiento
    public AudioClip fireClip;                // Sonido de disparo
    public AudioClip punchClip;               // Sonido de golpe
    public AudioClip damageClip;              // Sonido de recibir da�o
    public AudioClip deathClip;               // Sonido de muerte

    private AudioSource audioSource;          // AudioSource para reproducir sonidos

    // Referencia a la barra de vida
    public Image lifeBar;

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
        deathMenu = FindObjectOfType<DeathMenu>();  // Encuentra el men� de muerte

        // Buscar la barra de vida por su tag
        GameObject lifeBarObject = GameObject.FindWithTag("LifeBar");
        if (lifeBarObject != null)
        {
            lifeBar = lifeBarObject.GetComponent<Image>();
            lifeBar.fillAmount = currentHealth / maxHealth;
            Debug.Log("Barra de vida encontrada y asignada.");
        }
        else
        {
            Debug.LogWarning("No se encontr� el objeto con el tag 'LifeBar'.");
        }
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

        // Reproduce sonido de movimiento si el jugador se mueve
        if ((moveX != 0 || moveZ != 0) && !audioSource.isPlaying)
        {
            PlaySound(moveClip);
        }
    }

    void Fire()
    {
        // Verifica si se presiona el bot�n de disparo
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Dispara una bala
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Bullet>().damage = 10f; // Ajusta el da�o si es necesario

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
                Debug.Log("No golpe� a ning�n enemigo."); // Mensaje si no golpea
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log("Player ha recibido da�o. Vida actual: " + currentHealth);

        // Reproduce sonido de da�o
        PlaySound(damageClip);

        // Actualiza la barra de vida
        if (lifeBar != null)
        {
            lifeBar.fillAmount = currentHealth / maxHealth;
            Debug.Log("Barra de vida actualizada: " + lifeBar.fillAmount);
        }
        else
        {
            Debug.LogWarning("Referencia a la barra de vida es nula.");
        }

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
            deathMenu.ShowDeathMenu();  // Muestra el men� de muerte
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
            Debug.Log("Player se cur�. Vida actual: " + currentHealth);

            // Actualiza la barra de vida
            if (lifeBar != null)
            {
                lifeBar.fillAmount = currentHealth / maxHealth;
                Debug.Log("Barra de vida actualizada: " + lifeBar.fillAmount);
            }
        }
    }

    public void MoreDamage(float amount)
    {
        if (punchDamage < 50f)
        {
            punchDamage += amount;
            Debug.Log("Recibi� m�s da�o");
        }
    }

    // Funci�n para reproducir sonidos
    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
