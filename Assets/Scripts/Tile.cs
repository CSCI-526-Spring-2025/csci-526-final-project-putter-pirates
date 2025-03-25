using UnityEngine;
using UnityEngine.UIElements;


public class Tile : MonoBehaviour
{
    GameObject gc;
    Collider2D collider2d;
    public int index;
    public float rotation;

    void Start()
    {
        gc = GameObject.Find("GameController");
        collider2d = GetComponent<Collider2D>();
        rotation = transform.eulerAngles.z;
    }

    void Update()
    {
        if (!gc.GetComponent<GameController>().isRotateState) return;
        if (gc.GetComponent<GameController>().isPaused) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (collider2d.OverlapPoint(mousePosition))
            {
                transform.Rotate(Vector3.forward, 90);
                Debug.Log($"Tile clicked: Index = {index}");
            }
        }
    }
}