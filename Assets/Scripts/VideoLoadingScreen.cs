using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoLoadingScreen : MonoBehaviour
{
    [Header("Video Settings")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private bool skipVideoIfLoadingCompletes = false;
    [SerializeField] private bool waitForVideoToFinish = true;

    private AsyncOperation asyncLoadOperation;
    private bool isVideoFinished = false;
    private bool isSceneLoaded = false;

    private void Start()
    {
        // Configurar el evento para detectar cuando el video termine
        videoPlayer.loopPointReached += VideoFinished;
        
        // Iniciar la carga de la escena de forma asíncrona
        StartCoroutine(LoadSceneAsync());
        
        // Iniciar la reproducción del video
        videoPlayer.Play();
    }

    private void Update()
    {
        // Verificar si podemos cambiar a la escena cargada
        if (CanProceedToNextScene())
        {
            asyncLoadOperation.allowSceneActivation = true;
        }
    }

    private IEnumerator LoadSceneAsync()
    {
        // Comenzar a cargar la escena de forma asíncrona
        asyncLoadOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        
        // Impedir que la escena se active automáticamente cuando termine de cargar
        asyncLoadOperation.allowSceneActivation = false;
        
        // Esperar hasta que la escena esté cargada al 90%
        while (asyncLoadOperation.progress < 0.9f)
        {
            yield return null;
        }
        
        // La escena está lista para activarse
        isSceneLoaded = true;
        
        // Si está configurado para saltar el video cuando la carga está completa
        if (skipVideoIfLoadingCompletes && !waitForVideoToFinish)
        {
            asyncLoadOperation.allowSceneActivation = true;
        }
    }

    private void VideoFinished(VideoPlayer vp)
    {
        isVideoFinished = true;
    }

    private bool CanProceedToNextScene()
    {
        // Si no necesitamos esperar a que el video termine, activamos la escena tan pronto como esté cargada
        if (!waitForVideoToFinish)
        {
            return isSceneLoaded;
        }
        
        // De lo contrario, esperamos a que tanto el video termine como la escena esté cargada
        return isSceneLoaded && isVideoFinished;
    }
}