using UnityEngine;

public class PlaneScaler : MonoBehaviour
{
    public Vector2 referenceResolution = new Vector2(1920f, 1080f);
    public float baseWidth = 19.2f; // Ajusta según el tamaño base en unidades de Unity
    public float baseHeight = 10.8f; // Ajusta según el tamaño base en unidades de Unity

    void Update()
    {
        float widthScale = Screen.width / referenceResolution.x;
        float heightScale = Screen.height / referenceResolution.y;
        float scaleFactor = Mathf.Max(widthScale, heightScale); // Usa el mayor para cubrir toda la pantalla

        transform.localScale = new Vector3(baseWidth * scaleFactor, 1f, baseHeight * scaleFactor);
    }
}