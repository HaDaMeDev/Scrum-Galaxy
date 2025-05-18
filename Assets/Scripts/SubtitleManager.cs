using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject subtitlePanel;
    public TextMeshProUGUI subtitleText;

    private CanvasGroup canvasGroup;

    // Nombre de la escena donde no debe existir
    [SerializeField] private string excludedScene = "SampleScene";

    private void Awake()
    {
        // Verifica si ya existe una instancia
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Suscribirse al cambio de escenas
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Configurar el canvas group
        canvasGroup = subtitlePanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = subtitlePanel.AddComponent<CanvasGroup>();
        }

        subtitlePanel.SetActive(false);
    }

    // Método que se llama al cargar una nueva escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == excludedScene)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    // Métodos para mostrar subtítulos
    public void ShowText(string text, float duration)
    {
        StartCoroutine(ShowTextCoroutine(text, duration));
    }

    private IEnumerator ShowTextCoroutine(string text, float duration)
    {
        subtitlePanel.SetActive(true);
        subtitleText.text = text;
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(duration);

        canvasGroup.alpha = 0;
        subtitlePanel.SetActive(false);
    }

    public void ShowSubtitles(List<SubtitleLine> subtitles, AudioClip audioClip)
    {
        StartCoroutine(ShowSubtitlesCoroutine(subtitles, audioClip));
    }

    private IEnumerator ShowSubtitlesCoroutine(List<SubtitleLine> subtitles, AudioClip audioClip)
    {
        float startTime = Time.time;
        yield return new WaitForSeconds(0.1f);

        subtitlePanel.SetActive(true);

        foreach (SubtitleLine line in subtitles)
        {
            float timeToWait = line.startTime - (Time.time - startTime);
            if (timeToWait > 0)
                yield return new WaitForSeconds(timeToWait);

            subtitleText.text = line.text;
            canvasGroup.alpha = 1;

            yield return new WaitForSeconds(line.duration);
        }

        canvasGroup.alpha = 0;
        subtitlePanel.SetActive(false);
    }

    public void StopSubtitles()
    {
        StopAllCoroutines();
        subtitlePanel.SetActive(false);
    }
}
