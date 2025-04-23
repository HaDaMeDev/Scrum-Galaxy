using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Asigna el VideoPlayer aquí
    public string nextSceneName = "SampleScene"; // Escena a cargar después del video

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished; // Suscribe el evento para cuando termine
            videoPlayer.Play(); // Inicia el video
        }
        else
        {
            Debug.LogError("VideoPlayer no está asignado en " + gameObject.name);
            SceneManager.LoadScene(nextSceneName); // Carga la escena si no hay video
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName); // Carga la escena cuando el video termina
    }
}