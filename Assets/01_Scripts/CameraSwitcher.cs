using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera firstPersonCamera;    // C�mara de primera persona
    public Camera thirdPersonCamera;    // C�mara de tercera persona

    void Start()
    {
        // Iniciar con la c�mara de primera persona activa y la de tercera persona desactivada
        if (firstPersonCamera == null || thirdPersonCamera == null)
        {
            Debug.LogError("Las c�maras no est�n asignadas en el Inspector.");
            return;
        }

        firstPersonCamera.gameObject.SetActive(true);
        thirdPersonCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        // Cambiar entre las c�maras al presionar la tecla C
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCamera();
        }
    }

    void ToggleCamera()
    {
        // Verifica si las c�maras est�n asignadas antes de usarlas
        if (firstPersonCamera == null || thirdPersonCamera == null)
        {
            Debug.LogError("Una de las c�maras no est� asignada.");
            return;
        }

        // Alterna entre la c�mara de primera y tercera persona
        bool isFirstPersonActive = firstPersonCamera.gameObject.activeSelf;

        firstPersonCamera.gameObject.SetActive(!isFirstPersonActive);   // Activa la c�mara opuesta
        thirdPersonCamera.gameObject.SetActive(isFirstPersonActive);
    }
}
