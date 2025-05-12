using UnityEngine;

public class DynamicPosition : MonoBehaviour
{
    public Vector2 referencePosition = new Vector2(0f, 0f);
    public Vector2 referenceResolution = new Vector2(1920f, 1080f);

    void Update()
    {
        float xPos = (referencePosition.x / referenceResolution.x) * Screen.width;
        float yPos = (referencePosition.y / referenceResolution.y) * Screen.height;
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }
}