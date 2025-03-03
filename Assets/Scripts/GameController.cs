using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool isRotateState = true;
    public bool isGameState = true;
    GameObject ball;        // in "rotate state", the fakeball without physics would be activated (to prevent falling), 
    GameObject fakeBall;    // in "play state" the real ball with physics would be activated
    GameObject topLayer;    // the transparent layer to visually distinguish rotate mode and normal mode
    GameObject goalEffect;
    GameObject goal;
    ElectricityButton electricityButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        ball = GameObject.Find("Ball");
        fakeBall = GameObject.Find("FakeBall");
        topLayer = GameObject.Find("TopLayer");
        goalEffect = GameObject.Find("GoalEffect");
        goal = GameObject.Find("Goal");
        electricityButton = GameObject.Find("ElectricityButton").GetComponent<ElectricityButton>();

        ball.SetActive(!isRotateState);
        fakeBall.SetActive(isRotateState);
        topLayer.SetActive(isRotateState);
        goalEffect.SetActive(!isGameState);
    }

    // Update is called once per frame
    void Update()
    {
        // the state is toggled with Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            goal.GetComponent<Goal>().flipFreeze();
            ToggleState();
        }

        // R key resets the ball
        if(Input.GetKeyDown(KeyCode.R)) ResetLevel();
    }

    public void Sucess()
    {
        // would be called by Goal as the ball got to the goal
        ball.GetComponent<Ball>().Freeze();
        goalEffect.SetActive(true);
    }

    public void ToggleState()
    {
        isRotateState = !isRotateState;
        Debug.Log("isRotateState: " + isRotateState);

        ball.SetActive(!isRotateState);
        ball.transform.position = fakeBall.transform.position;
        ball.GetComponent<Ball>().SetStartPosition(ball.transform.position);
        ball.GetComponent<Ball>().ResetPosition();
        fakeBall.SetActive(isRotateState);
        topLayer.SetActive(isRotateState);
    }

    public void ResetLevel()
    {
        ball.GetComponent<Ball>().ResetPosition();
        goalEffect.GetComponent<ParticleSystem>().Clear();
        goalEffect.SetActive(false);
        electricityButton.ResetButton();
    }
}
