using UnityEngine;

[System.Serializable]
public class SubtitleLine
{
    public string text;
    public float startTime;  // Tiempo en segundos desde el inicio del audio
    public float duration;   // Duración en segundos que se muestra este subtítulo
}