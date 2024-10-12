using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera firstPersonCamera;    // Cámara de primera persona
    public Camera thirdPersonCamera;    // Cámara de tercera persona
    public Camera bossCamera;

    void Start()
    {
        // Iniciar con la cámara de primera persona activa y la de tercera persona desactivada
        if (firstPersonCamera == null || thirdPersonCamera == null)
        {
            Debug.LogError("Las cámaras del player no están asignadas en el Inspector.");
            return;
        }

        firstPersonCamera.gameObject.SetActive(true);
        thirdPersonCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        // Cambiar entre las cámaras al presionar la tecla C
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCamera();
        }
    }

    void ToggleCamera()
    {
        // Verifica si las cámaras están asignadas antes de usarlas
        if (firstPersonCamera == null || thirdPersonCamera == null)
        {
            Debug.LogError("Una de las cámaras no está asignada.");
            return;
        }

        // Alterna entre la cámara de primera y tercera persona
        bool isFirstPersonActive = firstPersonCamera.gameObject.activeSelf;

        firstPersonCamera.gameObject.SetActive(!isFirstPersonActive);   // Activa la cámara opuesta
        thirdPersonCamera.gameObject.SetActive(isFirstPersonActive);
    }

    public void AssignBossCamera(Camera newBossCamera)
    {
        bossCamera = newBossCamera;
    }

    public void SwitchToBossCamera()
    {
        if (bossCamera == null)
        {
            Debug.LogError("Boss camera is not assigned.");
            return;
        }

        // Deactivate both player cameras and activate the boss camera
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(false);
        bossCamera.gameObject.SetActive(true);
    }
}
