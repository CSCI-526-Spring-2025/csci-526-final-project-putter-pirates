using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool isRotateState = true;
    public bool isGameState = true;
    public bool isPaused = false;
    GameObject ball;        // in "rotate state", the fakeball without physics would be activated (to prevent falling), 
    GameObject fakeBall;    // in "play state" the real ball with physics would be activated
    //GameObject topLayer;    // the transparent layer to visually distinguish rotate mode and normal mode
    GameObject topLayer1;
    GameObject topLayer2;
    GameObject goalEffect;
    GameObject confettiEffect;
    Quaternion confettiQuat;
    GameObject goal;
    Trajectory trajectory;
    LevelUIManagement levelUIManagement;
    ElectricityManager electricityManager = null;
    FlipColorManager flipColorManager = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ball = GameObject.Find("Ball");
        fakeBall = GameObject.Find("FakeBall");
        // topLayer = GameObject.Find("TopLayer");
        topLayer1 = GameObject.Find("TopLayer1");
        topLayer2 = GameObject.Find("TopLayer2");
        //goalEffect = GameObject.Find("GoalEffect");
        confettiEffect = GameObject.Find("ConfettiEffect");
        goal = GameObject.Find("Goal");
        if (GameObject.Find("FlipColorManager") != null)
        {
            flipColorManager = GameObject.Find("FlipColorManager").GetComponent<FlipColorManager>();

        }

        trajectory = GameObject.Find("Trajectory").GetComponent<Trajectory>();
        levelUIManagement = GameObject.Find("CanvasMenu").GetComponent<LevelUIManagement>();
        if (GameObject.Find("ElectricityManager") != null)
        {
            electricityManager = GameObject.Find("ElectricityManager").GetComponent<ElectricityManager>();
        }

        ball.SetActive(!isRotateState);
        fakeBall.SetActive(isRotateState);
        // topLayer.SetActive(isRotateState);
        if (topLayer1 != null && topLayer2 != null)
        {
            topLayer1.SetActive(isRotateState);
            topLayer2.SetActive(isRotateState);
        }
        // goalEffect.SetActive(!isGameState);
        confettiEffect.SetActive(false);
        confettiQuat = confettiEffect.transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        // the state is toggled with Space
        if (Input.GetKeyDown(KeyCode.Space) && isGameState)
        {
            ToggleState();
        }

        // R key resets the ball
        if (Input.GetKeyDown(KeyCode.R) && isGameState) ResetLevel();

        confettiEffect.transform.rotation = confettiQuat;
    }

    public void Sucess()
    {
        // would be called by Goal as the ball got to the goal
        ball.GetComponent<Ball>().Freeze();
        ball.SetActive(false);
        // goalEffect.SetActive(true);
        confettiEffect.SetActive(true);
        levelUIManagement.StartLevelClearUIRoutine();
        isGameState = false;

        GameAnalytics.instance.AppendShotData();
        GameAnalytics.instance.AppendAimData();
        GameAnalytics.instance.TrackGoalReached();
    }

    public void ToggleState()
    {
        if (!isGameState) return;

        isRotateState = !isRotateState;
        Debug.Log("isRotateState: " + isRotateState);

        ball.SetActive(!isRotateState);
        ball.transform.position = fakeBall.transform.position;
        ball.GetComponent<Ball>().SetStartPosition(ball.transform.position);
        ball.GetComponent<Ball>().ResetPosition();
        fakeBall.SetActive(isRotateState);

        if (topLayer1 != null && topLayer2 != null)
        {
            topLayer1.SetActive(isRotateState);
            topLayer2.SetActive(isRotateState);
        }
        goal.GetComponent<Goal>().flipFreeze();
        if (isRotateState) trajectory.Hide();
        if (electricityManager != null) electricityManager.ResetButtons();

        if (flipColorManager != null)
        {
            flipColorManager.Flip(isRotateState);
        }

        Tile[] allTiles = FindObjectsOfType<Tile>();
        foreach (var tile in allTiles)
        {
            tile.RefreshColor();
        }
    }

    public void ResetLevel()
    {
        ball.GetComponent<Ball>().ResetPosition();
        //goalEffect.GetComponent<ParticleSystem>().Clear();
        //goalEffect.SetActive(false);
        confettiEffect.GetComponent<ParticleSystem>().Clear();
        confettiEffect.SetActive(false);
        isGameState = true;
        if (electricityManager != null) electricityManager.ResetButtons();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Debug.Log("pause toggled to: " + isPaused);
    }
}
