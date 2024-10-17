using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusic;  // Música de fondo
    private AudioSource audioSource;

    // Parámetros para controlar el volumen y si el sonido está en loop
    [Range(0f, 1f)] public float volume = 0.5f;  // Control de volumen (entre 0 y 1)
    public bool loopMusic = true;  // Control para repetir la música en bucle

    void Start()
    {
        // Verifica si el AudioSource existe, de lo contrario lo añade automáticamente
        if (GetComponent<AudioSource>() == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Asigna la música de fondo
        audioSource.clip = backgroundMusic;

        // Configura si la música se repetirá o no
        audioSource.loop = loopMusic;

        // Configura el volumen inicial
        audioSource.volume = volume;

        // Comienza a reproducir la música de fondo
        audioSource.Play();
    }

    // Función para ajustar el volumen en tiempo real desde otros scripts si es necesario
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp(newVolume, 0f, 1f);  // Asegura que el volumen esté entre 0 y 1
        audioSource.volume = volume;
    }
}