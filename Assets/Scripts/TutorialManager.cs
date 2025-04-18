using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject rotate1Hint;
    [SerializeField] GameObject changeState1Hint;
    [SerializeField] GameObject shootingHint;
    [SerializeField] GameObject resetBallHint;
    [SerializeField] GameObject changeState2Hint;
    [SerializeField] GameObject rotate2Hint;
    [SerializeField] GameObject changeState3Hint;
    [SerializeField] GameObject goodLuckHint;
    [SerializeField] GameObject congratsHint;
    [SerializeField] GameObject tile0;
    [SerializeField] GameObject tile1;
    [SerializeField] GameObject stateSwitchButton;
    [SerializeField] GameObject overlayMenu;
    [SerializeField] float waitingTime = 0.5f;
    Vector3 ballStartPosition;
    float[] rotate1Angles = {0, 0};
    float rotateTriggeredTime = 0;
    float resetTriggeredTime = 0;
    bool rotateTriggered = false;
    bool resetTriggered = false;

    enum TutorialState {
        Rotate1, ChangeState1, Shooting, ResetBall, ChangeState2, Rotate2, ChangeState3, GoodLuck, ReachGoal,
    }
    [SerializeField]
    TutorialState tutorialState;

    void Start()
    {
        tutorialState = TutorialState.Rotate1;
        gameController.enabled = false; // block switching state with space bar
        rotateTriggered = false;
    }

    void Update()
    {
        if(tutorialState == TutorialState.Rotate1){
            bool tile0_rotated = !Mathf.Approximately(tile0.transform.eulerAngles.z, 270);
            bool tile1_rotated = !Mathf.Approximately(tile1.transform.eulerAngles.z, 180);
            if((tile0_rotated || tile1_rotated) && !rotateTriggered){
                // a tile is rotated, wait 0.5s
                rotate1Hint.SetActive(false);
                tile0.GetComponent<Tile>().enabled = false;
                tile1.GetComponent<Tile>().enabled = false;
                rotateTriggeredTime = Time.time;
                rotateTriggered = true;
            }
            if(rotateTriggered && Time.time - rotateTriggeredTime > waitingTime){
                changeState1Hint.SetActive(true);

                stateSwitchButton.SetActive(true);
                gameController.enabled = true;

                rotateTriggered = false;
                tutorialState = TutorialState.ChangeState1;
            }
        }
        else if(tutorialState == TutorialState.ChangeState1){
            if(!gameController.isRotateState){
                // the state is changed
                changeState1Hint.SetActive(false);
                shootingHint.SetActive(true);

                tile0.GetComponent<Tile>().enabled = true;
                tile1.GetComponent<Tile>().enabled = true;
                gameController.enabled = false;
                stateSwitchButton.GetComponent<Button>().enabled = false;

                ballStartPosition = ball.transform.position;
                tutorialState = TutorialState.Shooting;
            }
        }
        else if(tutorialState == TutorialState.Shooting){
            float ball_velocity = ball.GetComponent<Rigidbody2D>().linearVelocity.magnitude;
            if(Vector3.Distance(ball.transform.position, ballStartPosition) > 0.1 && ball_velocity < 0.1){
                // the ball is shooted
                shootingHint.SetActive(false);
                resetBallHint.SetActive(true);

                ball.GetComponent<Ball>().enabled = false;
                
                resetTriggered = false;
                tutorialState = TutorialState.ResetBall;
            }
        }
        else if(tutorialState == TutorialState.ResetBall){
            if(Input.GetKeyDown(KeyCode.R)){
                gameController.ResetLevel();
                resetTriggeredTime = Time.time;
                resetTriggered = true;

                resetBallHint.SetActive(false);
            }
            if(resetTriggered && Time.time - resetTriggeredTime > waitingTime){
                changeState2Hint.SetActive(true);

                gameController.enabled = true;
                stateSwitchButton.GetComponent<Button>().enabled = true;
                
                tutorialState = TutorialState.ChangeState2;
            }
        }
        else if(tutorialState == TutorialState.ChangeState2){
            if(gameController.isRotateState){
                // the state is changed back to rotate
                changeState2Hint.SetActive(false);
                rotate2Hint.SetActive(true);

                ball.GetComponent<Ball>().enabled = true;
                gameController.enabled = false;
                stateSwitchButton.GetComponent<Button>().enabled = false;
                
                rotate1Angles[0] = tile0.transform.eulerAngles.z;
                rotate1Angles[1] = tile1.transform.eulerAngles.z;
                tutorialState = TutorialState.Rotate2;
            }
        }
        else if(tutorialState == TutorialState.Rotate2){
            bool tile0_rotated = !Mathf.Approximately(tile0.transform.eulerAngles.z, rotate1Angles[0]);
            bool tile1_rotated = !Mathf.Approximately(tile1.transform.eulerAngles.z, rotate1Angles[1]);
            if(tile0_rotated || tile1_rotated) {
                rotate2Hint.SetActive(false);
                changeState3Hint.SetActive(true);

                gameController.enabled = true;
                stateSwitchButton.GetComponent<Button>().enabled = true;

                tutorialState = TutorialState.ChangeState3;
            }
        }
        else if(tutorialState == TutorialState.ChangeState3){
            if(!gameController.isRotateState){
                // the state is changed back to game
                changeState3Hint.SetActive(false);
                goodLuckHint.SetActive(true);

                for(int i=0;i<overlayMenu.transform.childCount;i++){
                    overlayMenu.transform.GetChild(i).gameObject.SetActive(true);
                }

                tutorialState = TutorialState.GoodLuck;
            }
        }
        else if(tutorialState == TutorialState.GoodLuck){
            if(!gameController.isGameState) {
                // player reached the goal
                goodLuckHint.SetActive(false);
                congratsHint.SetActive(true);
                tutorialState = TutorialState.ReachGoal;
            }
        }
        else if(tutorialState == TutorialState.ReachGoal){

        }
        else{
            Debug.LogError($"Unknown tutorial state {tutorialState}");
        }
    }
}
