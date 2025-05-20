using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using TMPro;

public class VolumeController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Slider volumeSlider; // Referencia al Slider
    [SerializeField] private TextMeshProUGUI volumeText; // Referencia al texto
    [SerializeField] private Image volumeIcon; // Referencia a la imagen de volumen
    [SerializeField] private float volumeChangeStep = 0.1f; // Incremento/decremento por clic

    private static VolumeController instance; // Para el patrón Singleton
    private float currentVolume; // Volumen global

    void Awake()
    {
        // Implementar Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("VolumeController inicializado y marcado como DontDestroyOnLoad");
        }
        else
        {
            Debug.LogWarning("Instancia duplicada de VolumeController encontrada. Destruyendo...");
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Validar referencias
        if (volumeSlider == null) Debug.LogError("VolumeSlider no asignado en " + gameObject.name);
        if (volumeText == null) Debug.LogError("VolumeText no asignado en " + gameObject.name);
        if (volumeIcon == null) Debug.LogError("VolumeIcon no asignado en " + gameObject.name);

        // Cargar volumen guardado
        currentVolume = PlayerPrefs.GetFloat("GlobalVolume", 1f);
        if (volumeSlider != null)
        {
            volumeSlider.value = currentVolume;
            volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        UpdateVolumeText();
        ApplyVolume();
    }

    // Detectar clics izquierdo y derecho
    public void OnPointerClick(PointerEventData eventData)
    {
        if (volumeSlider == null)
        {
            Debug.LogError("Clic ignorado: VolumeSlider es nulo");
            return;
        }

        Debug.Log($"Clic detectado: {eventData.button} en {gameObject.name}");
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            volumeSlider.value = Mathf.Clamp01(volumeSlider.value + volumeChangeStep);
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            volumeSlider.value = Mathf.Clamp01(volumeSlider.value - volumeChangeStep);
        }

        currentVolume = volumeSlider.value;
        UpdateVolumeText();
        ApplyVolume();
    }

    // Aplicar el volumen a todas las fuentes de audio
    private void ApplyVolume()
    {
        if (volumeSlider == null)
        {
            Debug.LogError("No se puede aplicar volumen: VolumeSlider es nulo");
            return;
        }

        // Actualizar AudioListener
        AudioListener.volume = currentVolume;

        // Actualizar AudioSources
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in audioSources)
        {
            source.volume = currentVolume;
            Debug.Log($"Volumen aplicado a AudioSource en {source.gameObject.name}: {currentVolume}");
        }

        // Actualizar VideoPlayers
        VideoPlayer[] videoPlayers = FindObjectsOfType<VideoPlayer>();
        foreach (VideoPlayer player in videoPlayers)
        {
            player.SetDirectAudioVolume(0, currentVolume);
            Debug.Log($"Volumen aplicado a VideoPlayer en {player.gameObject.name}: {currentVolume}");
        }

        // Guardar volumen
        PlayerPrefs.SetFloat("GlobalVolume", currentVolume);
        PlayerPrefs.Save();
        Debug.Log($"Volumen global guardado: {currentVolume}");
    }

    // Actualizar el texto del volumen
    private void UpdateVolumeText()
    {
        if (volumeText == null)
        {
            Debug.LogError("No se puede actualizar texto: VolumeText es nulo");
            return;
        }
        volumeText.text =  Mathf.RoundToInt(currentVolume * 100) + "%";
    }

    // Para cambios manuales en el Slider
    public void OnSliderValueChanged(float value)
    {
        currentVolume = value;
        UpdateVolumeText();
        ApplyVolume();
    }

    // Sincronizar audio en nuevas escenas
    public void UpdateSceneAudio()
    {
        ApplyVolume();
    }
}