using UnityEngine;

public class PlaneScaler : MonoBehaviour
{
    [Header("Configuración de resolución base")]
    public Vector2 referenceResolution = new Vector2(1920f, 1080f);

    [Header("Tamaño base del plano (en unidades de Unity)")]
    public float baseWidth = 19.2f;
    public float baseHeight = 10.8f;

    private int lastScreenWidth;
    private int lastScreenHeight;

    void Start()
    {
        ScalePlane();
    }

    void Update()
    {
        // Escala solo si la resolución cambia
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            ScalePlane();
        }
    }

    private void ScalePlane()
    {
        // Validación para evitar divisiones por cero
        if (referenceResolution.x == 0 || referenceResolution.y == 0)
        {
            Debug.LogError("Reference resolution values cannot be zero.");
            return;
        }

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        float widthScale = Screen.width / referenceResolution.x;
        float heightScale = Screen.height / referenceResolution.y;
        float scaleFactor = Mathf.Max(widthScale, heightScale); // Asegura que cubra toda la pantalla

        if (float.IsInfinity(scaleFactor) || float.IsNaN(scaleFactor))
        {
            Debug.LogError("Invalid scale factor calculated.");
            return;
        }

        transform.localScale = new Vector3(baseWidth * scaleFactor, 1f, baseHeight * scaleFactor);
    }
}
