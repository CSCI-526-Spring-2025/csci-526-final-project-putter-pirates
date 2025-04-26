using UnityEngine;

public class Tile : MonoBehaviour
{
    GameObject gc;
    Collider2D collider2d;
    public int index;
    public float rotation;
    public bool isLocked = false;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        gc = GameObject.Find("GameController");
        collider2d = GetComponent<Collider2D>();
        rotation = transform.eulerAngles.z;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!gc.GetComponent<GameController>().isRotateState || gc.GetComponent<GameController>().isPaused) return;
        if (isLocked) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (collider2d.OverlapPoint(mousePosition))
            {
                transform.Rotate(Vector3.forward, 90);
            }
        }
    }

    public void LockTile()
    {
        isLocked = true;

        // Change to blue to show it’s locked
        if (spriteRenderer != null)
            // spriteRenderer.color = Color.gray;
            spriteRenderer.color = new Color(0.2f, 0.4f, 1f); // stronger blue

    }
}
