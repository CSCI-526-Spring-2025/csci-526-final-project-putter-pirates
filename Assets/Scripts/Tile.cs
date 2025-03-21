using UnityEngine;
using UnityEngine.UIElements;

// public class Tile : MonoBehaviour
// {
//     GameObject gc;
//     Collider2D collider2d;
//     public int index;

//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         gc = GameObject.Find("GameController");
//         collider2d = gameObject.GetComponent<Collider2D>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if(!gc.GetComponent<GameController>().isRotateState) return;

//         if (gc.GetComponent<GameController>().pause) return;

//         // if the mouse clicked on the tile's collider, then rotate the tile
//         if(Input.GetMouseButtonDown(0)) {
//             Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//             if(collider2d.OverlapPoint(mousePosition)) {
//                 transform.Rotate(Vector3.forward, 90);
//             }
//             // Debug.Log($"Tile clicked: Index = {index}");
//         }
//     }
    

//     void OnMouseDown()
//     {
//         Debug.Log($"Tile clicked: Index = {index}");
//     }
// }



public class Tile : MonoBehaviour
{
    GameObject gc;
    Collider2D collider2d;
    public int index;

    void Start()
    {
        gc = GameObject.Find("GameController");
        collider2d = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!gc.GetComponent<GameController>().isRotateState) return;
        if (gc.GetComponent<GameController>().pause) return;

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
