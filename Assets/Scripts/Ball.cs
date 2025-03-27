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
    GameController gameController;
    bool freezed = false;  // if the ball goes into the hole, it won't came back
    bool shooted = false;
    bool isAfterPlayModeMouseDown = true;
    float smallVelocityContinuousTime = 0;

    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        lastpath = GameObject.Find("LastPath");
        triangle = transform.Find("Triangle").gameObject;

        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // prevent the ball from falling before shoot

        triangle.SetActive(false);
    }

    void Update()
    {
        if (freezed || gameController.isRotateState) return;
        if (shooted)
        {
            if (rb.linearVelocity.magnitude < 0.2)
            {
                smallVelocityContinuousTime += Time.deltaTime;
                // if the velocity is low for more than 0.2 seconds, reset position
                if (smallVelocityContinuousTime > 0.2) gameController.ResetLevel();
            }
            else smallVelocityContinuousTime = 0;
            return;
        }

        rb.linearVelocity = Vector2.zero;
        if (Input.GetMouseButtonDown(0))
        {
            // when the mouse is down, we record the start position
            ms_down_pos = Input.mousePosition;
            isAfterPlayModeMouseDown = true;
        }

        // calculate the direction and magnitude of shooting
        Vector2 dir, dist;
        float meg, ag;
        dist = ms_down_pos - Input.mousePosition;
        dir = dist.normalized;
        meg = dist.magnitude / ((Screen.width + Screen.height) / 2);
        ag = Mathf.Acos(dir.x) * 180 / Mathf.PI * (dir.y > 0 ? 1 : -1) - 90;
        if (meg > 0.4) meg = 0.4f;

        if (Input.GetMouseButtonUp(0) && isAfterPlayModeMouseDown)
        {
            // if the mouse is released, we use the direction and magnitude to shoot the ball
            if (meg < 0.1) return; // if the force is too weak, then don't shoot
            triangle.SetActive(false);
            rb.gravityScale = 1.5f;
            rb.linearVelocity = meg * shoot_speed * dir;
            shooted = true;
            lastpath.GetComponent<LastPath>().StartRecording();
            GameAnalytics.instance.TrackShot();
            FindObjectOfType<TileInitializer>().PrintTilesRotationState();
            GameAnalytics.instance.AppendStateData();
            // GameAnalytics.instance.PrintTileStates();

        }
        else if (Input.GetMouseButton(0) && isAfterPlayModeMouseDown)
        {
            // if the mouse is held, we use the direction and magnitude to transform that triangle
            triangle.SetActive(true);
            triangle.transform.localScale = new Vector3(1, meg * 20, 1);
            triangle.transform.rotation = Quaternion.Euler(0, 0, ag);
            triangle.GetComponent<SpriteRenderer>().color = new Color(meg / 0.5f, 0, 1 - meg / 0.5f, 0.5f);
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
        rb.gravityScale = 0;
        shooted = false;
        freezed = false;
        isAfterPlayModeMouseDown = false;
        lastpath.GetComponent<LastPath>().StopRecording();
    }
}