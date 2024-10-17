using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;  // Desbloquea el cursor
        Cursor.visible = true;  // Muestra el cursor
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EscenaJuego()
    {
        SceneManager.LoadScene("Juego");
    }

    public void CerrarJuego()
    {
        Application.Quit();  // Cierra la aplicación
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // Detiene el juego en el editor
#endif
    }
}
