using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject rotate1Hint;
    [SerializeField] GameObject changeState1Hint;
    [SerializeField] GameObject shootingHint;
    [SerializeField] GameObject changeState2Hint;
    [SerializeField] GameObject rotate2Hint;
    [SerializeField] GameObject changeState3Hint;
    [SerializeField] GameObject goodLuckHint;
    [SerializeField] GameObject congratsHint;
    [SerializeField] GameObject tile0;
    [SerializeField] GameObject tile1;
    [SerializeField] GameObject stateSwitchButton;
    [SerializeField] GameObject OverlayMenu;
    Vector3 ballStartPosition;
    float[] rotate1Angles = {0, 0};


     enum TutorialState {
        Rotate1, ChangeState1, Shooting, ChangeState2, Rotate2, ChangeState3, GoodLuck, ReachGoal,
    }
    [SerializeField]
    TutorialState tutorialState;

    void Start()
    {
        tutorialState = TutorialState.Rotate1;
        gameController.enabled = false; // block switching state with space bar
    }

    void Update()
    {
        if(tutorialState == TutorialState.Rotate1){
            bool tile0_rotated = !Mathf.Approximately(tile0.transform.eulerAngles.z, 180);
            bool tile1_rotated = !Mathf.Approximately(tile1.transform.eulerAngles.z, 180);
            if(tile0_rotated || tile1_rotated){
                // a tile is rotated
                rotate1Hint.SetActive(false);
                changeState1Hint.SetActive(true);

                tile0.GetComponent<Tile>().enabled = false;
                tile1.GetComponent<Tile>().enabled = false;
                stateSwitchButton.SetActive(true);
                gameController.enabled = true;

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
            if(Vector3.Distance(ball.transform.position, ballStartPosition) > 1){
                // the ball is shooted
                shootingHint.SetActive(false);
                changeState2Hint.SetActive(true);

                ball.GetComponent<Ball>().enabled = false;
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

                for(int i=0;i<OverlayMenu.transform.childCount;i++){
                    OverlayMenu.transform.GetChild(i).gameObject.SetActive(true);
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

    void SetTileRotatable(bool enable)
    {
        tile0.GetComponent<Tile>().enabled = enable;
        tile1.GetComponent<Tile>().enabled = enable;
    }
}
