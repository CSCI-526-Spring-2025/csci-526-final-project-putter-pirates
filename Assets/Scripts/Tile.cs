using UnityEngine;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour
{
    GameObject gc;
    Collider2D collider2d;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gc = GameObject.Find("GameController");
        collider2d = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gc.GetComponent<GameController>().isRotateState) return;
        if(Input.GetMouseButtonDown(0)) {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(collider2d.OverlapPoint(mousePosition)) {
                transform.Rotate(Vector3.forward, 90);
            }
        }
    }
}
