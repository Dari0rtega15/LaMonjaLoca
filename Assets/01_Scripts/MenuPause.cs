using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    public GameObject pauseMenuUI; // Referencia al men� de pausa
    private bool isPaused = false;  // Estado de pausa

    void Start()
    {
        pauseMenuUI.SetActive(false); // Aseg�rate de que el men� de pausa est� desactivado al inicio
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Al presionar ESC
        {
            if (isPaused)
            {
                Resume(); // Reanuda el juego
            }
            else
            {
                Pause(); // Pausa el juego
            }
        }
    }

    public void Resume()
    {
        Debug.Log("Intentando reanudar el juego...");
        pauseMenuUI.SetActive(false); // Oculta el men� de pausa
        Time.timeScale = 1f; // Reanuda el tiempo del juego
        isPaused = false; // Cambia el estado a no pausado

        // Aseg�rate de que el cursor sea visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia la escena actual
    }

    public void Pause()
    {
        Debug.Log("Pausando el juego...");
        pauseMenuUI.SetActive(true); // Muestra el men� de pausa
        Time.timeScale = 0f; // Detiene el tiempo del juego
        isPaused = true; // Cambia el estado a pausado

        // Aseg�rate de que el cursor sea visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void QuitGame()
    {
        Application.Quit(); // Cierra la aplicaci�n
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Detiene el juego en el editor
#endif
    }
}   