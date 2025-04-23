using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip dialogueAudio;
    public float triggerDistance = 3f;
    
    [Header("Subtitles")]
    public List<SubtitleLine> subtitleLines = new List<SubtitleLine>();
    
    private AudioSource audioSource;
    private Transform playerTransform;
    private bool isTriggered = false;
    
    void Start()
    {
        // Get or add AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        // Configure AudioSource
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.clip = dialogueAudio;
        
        // Find player
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
        else
            Debug.LogWarning("No object with 'Player' tag found. Make sure to tag your player.");
    }
    
    void Update()
    {
        if (playerTransform == null) return;
        
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        
        // Player entered range
        if (distance <= triggerDistance && !isTriggered)
        {
            TriggerDialogue();
        }
        // Player left range
        else if (distance > triggerDistance && isTriggered)
        {
            StopDialogue();
        }
    }
    
    void TriggerDialogue()
    {
        isTriggered = true;
        
        // Play audio
        if (dialogueAudio != null)
            audioSource.Play();
        
        // Show subtitles if SubtitleManager exists
        if (SubtitleManager.Instance != null && subtitleLines.Count > 0)
        {
            SubtitleManager.Instance.ShowSubtitles(subtitleLines, dialogueAudio);
        }
    }
    
    void StopDialogue()
    {
        isTriggered = false;
        
        // Stop audio
        if (audioSource.isPlaying)
            audioSource.Stop();
        
        // Stop subtitles
        if (SubtitleManager.Instance != null)
            SubtitleManager.Instance.StopSubtitles();
    }
    
    // Visualize trigger range in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerDistance);
    }
}