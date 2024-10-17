using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueMenu : MonoBehaviour
{
    public GameObject continueMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        continueMenuUI.SetActive(false);
    }
    public void ShowDeathMenu()
    {
        Time.timeScale = 0;  // Pausa el juego
        continueMenuUI.SetActive(true);  // Muestra el menú de muerte
        Cursor.lockState = CursorLockMode.None;  // Desbloquea el cursor
        Cursor.visible = true;  // Muestra el cursor
    }

    public void BackToStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Juego");
    }
}
