using UnityEngine;

public class ElectricDoor : MonoBehaviour
{
    float fadedAlpha = 0.3f; // Semi-transparent when powered
    float normalAlpha = 1f;  // Fully visible normally
    BoxCollider2D boxCollider2D;
    SpriteRenderer spriteRenderer;
    ElectricityComponent electricityComponent;

    [SerializeField]
    bool powered;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        electricityComponent = transform.Find("ElectricityTrigger").GetComponent<ElectricityComponent>();
    }

    void Update()
    {
        powered = electricityComponent.isCharged;

        if (powered)
        {
            // When powered: fade slightly and disable collision
            boxCollider2D.enabled = false;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, fadedAlpha);
        }
        else
        {
            // When not powered: normal appearance and block player
            boxCollider2D.enabled = true;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, normalAlpha);
        }
    }
}
