using UnityEngine;


public class Ball : MonoBehaviour
{
    public float shoot_speed = 50;
    public GameObject shadow;
    public Trajectory trajectory;

    public float impactForce = 100;

    GameObject triangle;
    GameObject lastpath;
    
    Rigidbody2D rb;
    Vector3 ms_down_pos;
    Vector3 startPosition;
    Vector3 referencePosition; // for measuring ball velocity after shoot
    GameController gameController;
    bool freezed = false;  // if the ball goes into the hole, it won't came back
    bool shooted = false;
    bool isAfterPlayModeMouseDown = true;
    float lastVelocityMeasurementTime = 0;

    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        lastpath = GameObject.Find("LastPath");
        // triangle = transform.Find("Triangle").gameObject;
        trajectory = GameObject.Find("Trajectory").GetComponent<Trajectory>();
        
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // prevent the ball from falling before shoot

        // triangle.SetActive(false);
        trajectory.Hide();
    }

    void Update()
    {
        if (freezed || gameController.isRotateState || gameController.isPaused)
        {
            trajectory.Hide();
            rb.linearVelocity = Vector2.zero;
            ms_down_pos = Input.mousePosition;
            return;
        }
        if (shooted)
        {
            if(Time.time - lastVelocityMeasurementTime > 0.2) {
                float averageVelocity = (transform.position - referencePosition).magnitude / 0.2f;
                lastVelocityMeasurementTime = Time.time;
                referencePosition = transform.position;

                // if the velocity is low, reset position
                if(averageVelocity < 0.1) gameController.ResetLevel();
            }
            return;
        }

        rb.linearVelocity = Vector2.zero;
        if (Input.GetMouseButtonDown(0))
        {
            // when the mouse is down, we record the start position
            ms_down_pos = Input.mousePosition;
            isAfterPlayModeMouseDown = true;

            // trajectory.Show();
        }

        // calculate the direction and magnitude of shooting
        Vector2 dir, dist;
        float meg, ag;
        dist = ms_down_pos - Input.mousePosition;
        dir = dist.normalized;
        meg = dist.magnitude / ((Screen.width + Screen.height) / 2);
        ag = Mathf.Acos(dir.x) * 180 / Mathf.PI * (dir.y > 0 ? 1 : -1) - 90;
        if (meg > 0.4) meg = 0.4f;

        Vector2 force = meg * shoot_speed * dir;

        if (Input.GetMouseButtonUp(0) && isAfterPlayModeMouseDown)
        {
            trajectory.Hide();
            // if the mouse is released, we use the direction and magnitude to shoot the ball
            if (meg < 0.1) return; // if the force is too weak, then don't shoot
            // triangle.SetActive(false);
            
            rb.gravityScale = 1.5f;
            rb.linearVelocity = meg * shoot_speed * dir;
            
            //rb.AddForce(force, ForceMode2D.Force);
            //Debug.Log("added force: " + force);
            lastVelocityMeasurementTime = Time.time;
            referencePosition = transform.position;

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
            if(meg <= 0) return;
            trajectory.Show();
            trajectory.UpdateDots(rb.position, force);
            //triangle.SetActive(true);
            //triangle.transform.localScale = new Vector3(1, meg * 20, 1);
            //triangle.transform.rotation = Quaternion.Euler(0, 0, ag);
            //triangle.GetComponent<SpriteRenderer>().color = new Color(meg / 0.5f, 0, 1 - meg / 0.5f, 0.5f);
        }
    }

    public void SetStartPosition(Vector3 pos)
    {
        startPosition = pos;
    }

    public void Freeze()
    {
        freezed = true;
        rb.simulated = false;
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;
        shooted = false;
        freezed = false;
        rb.simulated = true;
        isAfterPlayModeMouseDown = false;
        lastpath.GetComponent<LastPath>().StopRecording();
    }
}