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
    [SerializeField] GameObject UITour;
    [SerializeField] GameObject goodLuckHint;
    [SerializeField] GameObject congratsHint;
    [SerializeField] GameObject tile0;
    [SerializeField] GameObject tile1;
    [SerializeField] GameObject stateSwitchButton;
    [SerializeField] GameObject overlayMenu;
    [SerializeField] GameObject popupMenu;
    [SerializeField] float waitingTime = 0.5f;
    Vector3 ballStartPosition;
    float[] rotate1Angles = {0, 0};
    float rotateTriggeredTime = 0;
    float resetTriggeredTime = 0;
    float UITourStartTime = 0;
    float menuShowTime = 0;
    bool rotateTriggered = false;
    bool resetTriggered = false;
    bool menuClicked = false;
    bool closeMenuClicked = false;
    bool gotIt = false;

    enum TutorialState {
        Rotate1, ChangeState1, Shooting, ResetBall, ChangeState2, Rotate2, ChangeState3, UITour, GoodLuck, ReachGoal, Skipped,
    }
    enum UITourState {
        Wait, ClickMenu, ShowLevelSelect, ShowDotSlider, CloseMenu, ShowHint, End
    }
    [SerializeField]
    TutorialState tutorialState;
    UITourState uiTourState;

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
                UITour.SetActive(true);

                for(int i=0;i<overlayMenu.transform.childCount;i++){
                    overlayMenu.transform.GetChild(i).gameObject.SetActive(true);
                }

                gameController.enabled = false;
                ball.GetComponent<Ball>().enabled = false;

                UITourStartTime = Time.time;
                uiTourState = UITourState.Wait;
                tutorialState = TutorialState.UITour;
            }
        }
        else if(tutorialState == TutorialState.UITour){
            UITourUpdate();

            if(uiTourState == UITourState.End) {
                UITour.SetActive(false);
                goodLuckHint.SetActive(true);

                EnableLevelSelect();
                gameController.enabled = true;
                ball.GetComponent<Ball>().enabled = true;

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
        else if(tutorialState == TutorialState.ReachGoal || tutorialState == TutorialState.Skipped){

        }
        else{
            Debug.LogError($"Unknown tutorial state {tutorialState}");
        }
    }

    void UITourUpdate()
    {
        if(uiTourState == UITourState.Wait){
            if(Time.time - UITourStartTime >= waitingTime){
                UITour.transform.Find("ClickMenuHint").gameObject.SetActive(true);
                menuClicked = false;
                uiTourState = UITourState.ClickMenu;
            }
        }
        else if(uiTourState == UITourState.ClickMenu){
            if(menuClicked){
                UITour.transform.Find("ClickMenuHint").gameObject.SetActive(false);
                UITour.transform.Find("ShowLevelSelectHint").gameObject.SetActive(true);

                menuShowTime = Time.time;
                uiTourState = UITourState.ShowLevelSelect;
            }
        }
        else if(uiTourState == UITourState.ShowLevelSelect){
            if(gotIt){
                UITour.transform.Find("ShowLevelSelectHint").gameObject.SetActive(false);
                UITour.transform.Find("ShowDotSliderHint").gameObject.SetActive(true);

                gotIt = false;
                
                uiTourState = UITourState.ShowDotSlider;
            }
        }
        else if(uiTourState == UITourState.ShowDotSlider){
            if(gotIt){
                UITour.transform.Find("ShowDotSliderHint").gameObject.SetActive(false);
                UITour.transform.Find("CloseMenuHint").gameObject.SetActive(true);

                closeMenuClicked = false;
                
                uiTourState = UITourState.CloseMenu;
            }
        }
        else if(uiTourState == UITourState.CloseMenu){
            if(closeMenuClicked){
                UITour.transform.Find("CloseMenuHint").gameObject.SetActive(false);
                UITour.transform.Find("ShowHintHint").gameObject.SetActive(true);
                
                gotIt = false;

                uiTourState = UITourState.ShowHint;
            }
        }
        else if(uiTourState == UITourState.ShowHint){
            if(gotIt){
                UITour.transform.Find("ShowHintHint").gameObject.SetActive(false);
                
                uiTourState = UITourState.End;
            }
        }
    }

    void EnableLevelSelect()
    {
        for(int i=1;i<=12;i++) 
            popupMenu.transform.Find($"Level{i}").gameObject.GetComponent<Button>().interactable = true;
    }

    public void SkipTutorial()
    {
        tutorialState = TutorialState.Skipped;
        rotate1Hint.SetActive(false);
        gameController.enabled = true;
        for(int i=0;i<overlayMenu.transform.childCount;i++){
            overlayMenu.transform.GetChild(i).gameObject.SetActive(true);
        }
        EnableLevelSelect();
    }

    public void OnMenuClicked()
    {
        menuClicked = true;
    }

    public void OnGotItBtnClicked()
    {
        gotIt = true;
    }

    public void OnCloseMenuClicked()
    {
        closeMenuClicked = true;
    }
}
