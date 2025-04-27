using UnityEngine;

public class Tile : MonoBehaviour
{
    public int index;
    public float rotation; // Original rotation at Start
    public bool isLocked = false;
    public int goalState;  // Number of 90° clicks needed to reach goal

    private SpriteRenderer spriteRenderer;
    private GameObject gc;
    private Collider2D collider2d;

    void Start()
    {
        gc = GameObject.Find("GameController");
        collider2d = GetComponent<Collider2D>();
        rotation = Mathf.Round(transform.eulerAngles.z % 360f); // Save clean starting rotation
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (isLocked && spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.2f, 0.4f, 1f);
        }
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
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.2f, 0.4f, 1f); // Blue color to show locked
        }
    }

    public void SetToGoalState()
    {
        float targetRotation = (rotation + (goalState * 90f)) % 360f;
        transform.eulerAngles = new Vector3(0, 0, targetRotation);
    }

    public bool IsAtGoalState()
    {
        float currentRotation = Mathf.Round(transform.eulerAngles.z % 360f);
        float diff = (currentRotation - rotation + 360f) % 360f;
        int state = Mathf.RoundToInt(diff / 90f) % 4;
        return state == goalState;
    }

    public void RefreshColor()
    {
        if (isLocked && spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.2f, 0.4f, 1f); // Blue if locked
        }
    }

}
