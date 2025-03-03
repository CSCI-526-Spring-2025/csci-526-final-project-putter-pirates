using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    public float speed = 1.5f;  // Speed of movement
    public float height = 4f; // Movement range

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Move up and down using a sine wave
        transform.position = startPos + new Vector3(0, Mathf.Sin(Time.time * speed) * height, 0);
    }
}
