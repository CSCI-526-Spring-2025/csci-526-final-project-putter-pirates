using Unity.VisualScripting;
using UnityEngine;


public class Ball : MonoBehaviour
{
    public float shoot_speed = 50;
    public GameObject shadow;

    GameObject triangle;
    GameObject lastpath;
    Rigidbody2D rb;
    Vector3 ms_down_pos;
    Vector3 startPosition;
    bool freezed = false;  // if the ball goes into the hole, it won't came back
    bool shooted = false;

    void Start()
    {
        lastpath = GameObject.Find("LastPath");
        triangle = transform.Find("Triangle").gameObject;

        rb = gameObject.GetComponent<Rigidbody2D>();

        triangle.SetActive(false);
    }

    void Update()
    {
        if(freezed) return;
        if(shooted) 
        {
            if(rb.linearVelocity.magnitude < 0.2) ResetPosition();
            return;
        }

        if(Input.GetMouseButtonDown(0)){
            // when the mouse is down, we record the start position
            ms_down_pos = Input.mousePosition;
        }

        // calculate the direction and magnitude of shooting
        Vector2 dir, dist;
        float meg, ag;
        dist = ms_down_pos - Input.mousePosition;
        dir = dist.normalized;
        meg = dist.magnitude / ((Screen.width + Screen.height) / 2);
        ag = Mathf.Acos(dir.x) * 180 / Mathf.PI * (dir.y>0?1:-1) - 90;
        if(meg > 0.4) meg = 0.4f;

        if(Input.GetMouseButtonUp(0)){
            // if the mouse is released, we use the direction and magnitude to shoot the ball
            triangle.SetActive(false);
            rb.linearVelocity = dir * meg * shoot_speed;
            shooted = true;
            lastpath.GetComponent<LastPath>().StartRecording();
        }
        else if(Input.GetMouseButton(0)){
            // if the mouse is held, we use the direction and magnitude to transform that triangle
            triangle.SetActive(true);
            triangle.transform.localScale = new Vector3(1, meg * 20, 1);
            triangle.transform.rotation = Quaternion.Euler(0,0,ag);
            triangle.GetComponent<SpriteRenderer>().color = new Color(meg/0.5f, 0, 1-meg/0.5f, 0.5f);
        }
    }

    public void SetStartPosition(Vector3 pos)
    {
        startPosition = pos;
    }

    public void Freeze()
    {
        freezed = true;
        rb.linearVelocity = Vector2.zero;
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;
        shooted = false;
        freezed = false;
        lastpath.GetComponent<LastPath>().StopRecording();
    }
}
