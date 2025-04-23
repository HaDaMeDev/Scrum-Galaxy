using UnityEngine;

[RequireComponent(typeof(VideoLoadingScreen))]
public class VideoSceneInitializer : MonoBehaviour
{
    private void Awake()
    {
        // Obtener el componente VideoLoadingScreen
        VideoLoadingScreen videoLoadingScreen = GetComponent<VideoLoadingScreen>();
        
        // Establecer la escena a cargar desde la variable estática
        // Usar reflexión para acceder al campo privado sceneToLoad
        System.Reflection.FieldInfo sceneToLoadField = typeof(VideoLoadingScreen).GetField("sceneToLoad", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
        if (sceneToLoadField != null)
        {
            // Asignar el nombre de la escena
            sceneToLoadField.SetValue(videoLoadingScreen, SceneLoader.NextSceneName);
        }
        else
        {
            Debug.LogError("No se pudo acceder al campo 'sceneToLoad' en VideoLoadingScreen. " +
                          "Verifica que el nombre del campo no haya cambiado.");
        }
    }
}