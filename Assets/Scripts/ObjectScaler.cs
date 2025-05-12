using UnityEngine;

public class ObjectScaler : MonoBehaviour
{
    public float baseScale = 1f;
    public Vector2 referenceResolution = new Vector2(1920f, 1080f);

    void Update()
    {
        float scaleFactor = Mathf.Min(Screen.width / referenceResolution.x, Screen.height / referenceResolution.y);
        transform.localScale = Vector3.one * baseScale * scaleFactor;
    }
}