using UnityEngine;

public class LastPath : MonoBehaviour
{
    public GameObject dot;
    public bool recording = false;  // only instantiate dots when recording is on
    GameObject ball;
    Vector2 lastPosition;
    float lastTime;

    [SerializeField]
    float minDotDistance = 0.05f;
    [SerializeField]
    float timeDistance = 0.05f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ball = GameObject.Find("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        if(!recording) return;

        // if distance and time difference from last dot are both large enough, we draw a new dot
        if(Vector2.Distance(lastPosition, ball.transform.position) > minDotDistance && Time.time-lastTime > timeDistance) {
            GameObject newDot = Instantiate(dot, gameObject.transform);
            newDot.transform.position = ball.transform.position;
            lastPosition = ball.transform.position;
            lastTime = Time.time;
        }
    }

    public void StartRecording()
    {
        recording = true;
        foreach (Transform child in transform) Destroy(child.gameObject);
        lastPosition = ball.transform.position;
        lastTime = Time.time;
    }

    public void StopRecording()
    {
        recording = false;
    }
}
