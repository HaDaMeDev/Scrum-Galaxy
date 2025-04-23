using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Tecla que cerrará la aplicación")]
    public KeyCode exitKey = KeyCode.Space; // Cambiado a Space para evitar conflicto
    [Tooltip("¿Mostrar mensaje de confirmación en la consola?")]
    public bool showDebugMessage = true;

    [Tooltip("Nombre de la escena a la que se desea regresar")]
    public string targetSceneName = "SampleScene";
    
    [Tooltip("Tecla que activará el regreso a la escena")]
    public KeyCode returnKey = KeyCode.Escape;

    public float speed = 5f;
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Manejo de movimiento
        float moveX = Input.GetAxis("Horizontal"); // A/D o flechas izquierda/derecha
        float moveZ = Input.GetAxis("Vertical");   // W/S o flechas arriba/abajo

        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);

        // Verificar teclas especiales
        ExitApplication();
        ReturnToWorld();
    }
    
    void ExitApplication() // Corregido el nombre del método para seguir convenciones de C#
    {
        if (Input.GetKeyDown(exitKey))
        {
            if (showDebugMessage)
            {
                Debug.Log("Saliendo de la aplicación...");
            }
            
            // En el editor de Unity, detener la reproducción
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            // En una build, cerrar la aplicación
            #else
                Application.Quit();
            #endif
        }
    }

    void ReturnToWorld() // Corregido el nombre del método para seguir convenciones de C#
    {
        if (Input.GetKeyDown(returnKey))
        {
            if (showDebugMessage)
            {
                Debug.Log("Regresando a la escena: " + targetSceneName);
            }
            
            // Cargar la escena especificada
            SceneManager.LoadScene(targetSceneName);
        }
    }
}