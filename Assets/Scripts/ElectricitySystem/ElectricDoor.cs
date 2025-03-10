using UnityEngine;

public class ElectricDoor : MonoBehaviour
{
    Color openedColor = new Color(0.368f, 0, 0.671f, 0.1f);
    Color closedColor = new Color(0.368f, 0, 0.671f, 1);
    BoxCollider2D boxCollider2D;
    SpriteRenderer spriteRenderer;
    ElectricityComponent electricityComponent;

    [SerializeField]
    bool powered;

    void Start()
    {
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        electricityComponent = transform.Find("ElectricityTrigger").GetComponent<ElectricityComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        bool powered = electricityComponent.isCharged;
        boxCollider2D.enabled = !powered;
        spriteRenderer.color = powered? openedColor : closedColor;
    }
}
