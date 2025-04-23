using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private string videoLoadingSceneName = "VideoLoadingScene";
    
    // Variable estática para almacenar el nombre de la escena a cargar después del video
    public static string NextSceneName { get; private set; }

    /// <summary>
    /// Carga una escena a través de la pantalla de carga con video
    /// </summary>
    /// <param name="sceneName">Nombre de la escena a cargar</param>
    public void LoadSceneWithVideo(string sceneName)
    {
        // Guardar el nombre de la escena que queremos cargar
        NextSceneName = sceneName;
        
        // Cargar la escena de carga con video
        SceneManager.LoadScene(videoLoadingSceneName);
    }
    
    /// <summary>
    /// Carga una escena a través de la pantalla de carga con video
    /// </summary>
    /// <param name="sceneIndex">Índice de la escena a cargar</param>
    public void LoadSceneWithVideo(int sceneIndex)
    {
        // Convertir el índice al nombre de la escena
        string sceneName = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
        sceneName = System.IO.Path.GetFileNameWithoutExtension(sceneName);
        
        // Cargar usando el método que acepta nombre
        LoadSceneWithVideo(sceneName);
    }
}