using UnityEngine;

public class ElectricDoor : MonoBehaviour
{
    float fadedAlpha = 0.3f; // Faded but still visible
    float normalAlpha = 1f;  // Fully visible
    BoxCollider2D boxCollider2D;
    SpriteRenderer spriteRenderer;
    Animator animator;
    ElectricityComponent electricityComponent;

    [SerializeField]
    bool powered;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        electricityComponent = transform.Find("ElectricityTrigger").GetComponent<ElectricityComponent>();
    }

    void Update()
    {
        powered = electricityComponent.isCharged;

        if (powered)
        {
            // Powered: Fade and freeze
            boxCollider2D.enabled = false; // Allow passage
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, fadedAlpha);
            if (animator.enabled) animator.enabled = false; // Freeze animation
        }
        else
        {
            // Not powered: Normal
            boxCollider2D.enabled = true; // Block passage
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, normalAlpha);
            if (!animator.enabled) animator.enabled = true; // Resume animation
        }
    }
}
