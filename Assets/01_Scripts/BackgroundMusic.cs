using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip backgroundMusic;  // M�sica de fondo
    private AudioSource audioSource;

    // Par�metros para controlar el volumen y si el sonido est� en loop
    [Range(0f, 1f)] public float volume = 0.5f;  // Control de volumen (entre 0 y 1)
    public bool loopMusic = true;  // Control para repetir la m�sica en bucle

    void Start()
    {
        // Verifica si el AudioSource existe, de lo contrario lo a�ade autom�ticamente
        if (GetComponent<AudioSource>() == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Asigna la m�sica de fondo
        audioSource.clip = backgroundMusic;

        // Configura si la m�sica se repetir� o no
        audioSource.loop = loopMusic;

        // Configura el volumen inicial
        audioSource.volume = volume;

        // Comienza a reproducir la m�sica de fondo
        audioSource.Play();
    }

    // Funci�n para ajustar el volumen en tiempo real desde otros scripts si es necesario
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp(newVolume, 0f, 1f);  // Asegura que el volumen est� entre 0 y 1
        audioSource.volume = volume;
    }
}