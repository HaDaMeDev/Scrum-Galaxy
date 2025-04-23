using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Asegúrate de que este script se compila después de la definición de SubtitleLine
[RequireComponent(typeof(AudioSource))]
public class CharacterDialogueExample : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip characterVoice;
    
    [Header("Dialogue Settings")]
    public List<SubtitleLine> subtitleLines = new List<SubtitleLine>();
    
    private AudioSource audioSource;
    private Transform playerTransform;
    public float triggerDistance = 3f;
    private bool isPlayerInRange = false;
    private bool dialogueActivated = false;
    
    void Start()
    {
        // Configurar audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Configuramos ajustes de audio
        audioSource.spatialBlend = 1f;  // Audio 3D
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 10f;
        audioSource.clip = characterVoice;
        
        // Encontrar al jugador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("No se encontró objeto con tag 'Player'. Asegúrate de asignar este tag a tu jugador.");
        }
    }
    
    void Update()
    {
        if (playerTransform == null) return;
        
        // Verificar distancia al jugador
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        
        // El jugador entró en rango
        if (distance <= triggerDistance && !isPlayerInRange)
        {
            isPlayerInRange = true;
            StartDialogue();
        }
        // El jugador salió del rango
        else if (distance > triggerDistance && isPlayerInRange)
        {
            isPlayerInRange = false;
            StopDialogue();
        }
    }
    
    void StartDialogue()
    {
        if (dialogueActivated) return;
        
        // Verificar si el SubtitleManager existe
        if (SubtitleManager.Instance == null)
        {
            Debug.LogError("SubtitleManager no encontrado. Asegúrate de tener un objeto con el script SubtitleManager en la escena.");
            return;
        }
        
        dialogueActivated = true;
        
        // Reproducir audio
        if (audioSource != null && characterVoice != null)
        {
            audioSource.Play();
        }
        
        // Activar subtítulos
        SubtitleManager.Instance.ShowSubtitles(subtitleLines, characterVoice);
    }
    
    void StopDialogue()
    {
        if (!dialogueActivated) return;
        
        dialogueActivated = false;
        
        // Detener audio
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        
        // Detener subtítulos
        if (SubtitleManager.Instance != null)
        {
            SubtitleManager.Instance.StopSubtitles();
        }
    }
    
    // Para depuración - visualizar el rango de activación
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, triggerDistance);
    }
}