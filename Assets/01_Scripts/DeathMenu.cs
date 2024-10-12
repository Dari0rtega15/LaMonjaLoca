using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathMenu : MonoBehaviour
{
    public GameObject deathMenuUI;  // Referencia al panel del menú de muerte

    void Start()
    {
        // Asegúrate de que el menú de muerte esté oculto al inicio
        deathMenuUI.SetActive(false);
    }

    public void ShowDeathMenu()
    {
        Time.timeScale = 0;  // Pausa el juego
        deathMenuUI.SetActive(true);  // Muestra el menú de muerte
        Cursor.lockState = CursorLockMode.None;  // Desbloquea el cursor
        Cursor.visible = true;  // Muestra el cursor
    }

    public void RestartGame()
    {
        Time.timeScale = 1;  // Reanuda el juego
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia la escena actual
    }

    public void QuitGame()
    {
        Application.Quit();  // Cierra la aplicación
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Detiene el juego en el editor
#endif
    }
}
