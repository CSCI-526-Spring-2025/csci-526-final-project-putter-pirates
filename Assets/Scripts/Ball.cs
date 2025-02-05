using Unity.VisualScripting;
using UnityEngine;


public class Ball : MonoBehaviour
{
    public float shoot_speed = 50;
    public GameObject shadow;

    GameObject triangle;
    GameObject gc;
    GameObject lastpath;
    Rigidbody2D rb;
    Vector3 ms_down_pos;
    Vector2 original_pos;
    bool freezed = false;
    bool shooted = false;

    void Start()
    {
        gc = GameObject.Find("GameController");
        lastpath = GameObject.Find("LastPath");

        rb = gameObject.GetComponent<Rigidbody2D>();
        triangle = transform.Find("Triangle").gameObject;
        triangle.SetActive(false);
        original_pos = transform.position;
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
            ms_down_pos = Input.mousePosition;
        }

        Vector2 dir, dist;
        float meg, ag;
        dist = ms_down_pos - Input.mousePosition;
        dir = dist.normalized;
        meg = dist.magnitude / ((Screen.width + Screen.height) / 2);
        ag = Mathf.Acos(dir.x) * 180 / Mathf.PI * (dir.y>0?1:-1) - 90;
        if(meg > 0.4) meg = 0.4f;

        if(Input.GetMouseButtonUp(0)){
            triangle.SetActive(false);
            rb.linearVelocity = dir * meg * shoot_speed;
            shooted = true;
            lastpath.GetComponent<LastPath>().StartRecording();
        }
        else if(Input.GetMouseButton(0)){
            triangle.SetActive(true);
            triangle.transform.localScale = new Vector3(1, meg * 20, 1);
            triangle.transform.rotation = Quaternion.Euler(0,0,ag);
            triangle.GetComponent<SpriteRenderer>().color = new Color(meg/0.5f, 0, 1-meg/0.5f, 0.5f);
        }
    }

    public void Freeze()
    {
        freezed = true;
        rb.linearVelocity = Vector2.zero;
    }

    public void ResetPosition()
    {
        transform.position = original_pos;
        rb.linearVelocity = Vector2.zero;
        shooted = false;
        freezed = false;
        lastpath.GetComponent<LastPath>().StopRecording();
    }
}
