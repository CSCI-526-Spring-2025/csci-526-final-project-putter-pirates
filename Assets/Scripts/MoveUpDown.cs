using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    public float speed = 1.5f;  // Speed of movement
    public float height = 4f;   // Max movement range
    public float margin = 0.5f; // Margin to extend movement beyond the tile boundary

    private Vector3 startPos;
    private float movementLimit;
    private Transform tileTransform;

    void Start()
    {
        startPos = transform.position; // Store initial world position
        tileTransform = transform.parent; // Assuming moving group is a child of the tile

        if (tileTransform != null)
        {
            // Get tile size dynamically (assuming it has a Renderer)
            Renderer tileRenderer = tileTransform.GetComponent<Renderer>();
            if (tileRenderer != null)
            {
                float tileHeight = tileRenderer.bounds.size.y;
                movementLimit = tileHeight / 2 + margin; // Extend the limit by the margin
            }
            else
            {
                movementLimit = height; // Use the provided height if no tileRenderer is found
            }
        }
    }

    void Update()
    {
        // Calculate movement using sine wave
        float movementOffset = Mathf.Sin(Time.time * speed) * height;

        // Compute new position along local Y-axis
        Vector3 newPos = startPos + tileTransform.up * movementOffset;

        // Apply the offset with extended margin in all directions
        transform.position = tileTransform.position + tileTransform.up * Mathf.Clamp(movementOffset, -movementLimit - margin, movementLimit + margin);
    }
}
