using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance { get; private set; }
    
    [Header("UI References")]
    public GameObject subtitlePanel;
    public TextMeshProUGUI subtitleText;
    
    private CanvasGroup canvasGroup;
    
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Get or add CanvasGroup
        canvasGroup = subtitlePanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = subtitlePanel.AddComponent<CanvasGroup>();
        }
        
        // Hide panel at start
        subtitlePanel.SetActive(false);
    }
    
    // Simple method to show text
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
    
    // Method to show subtitles with an audio clip
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
            // Wait until it's time for this line
            float timeToWait = line.startTime - (Time.time - startTime);
            if (timeToWait > 0)
                yield return new WaitForSeconds(timeToWait);
            
            // Show this line
            subtitleText.text = line.text;
            canvasGroup.alpha = 1;
            
            // Wait for duration
            yield return new WaitForSeconds(line.duration);
        }
        
        // Hide panel
        canvasGroup.alpha = 0;
        subtitlePanel.SetActive(false);
    }
    
    public void StopSubtitles()
    {
        StopAllCoroutines();
        subtitlePanel.SetActive(false);
    }
}