using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject rotateTileHint;
    [SerializeField] GameObject changeStateHint;
    [SerializeField] GameObject shootBallHint;
    [SerializeField] GameObject goodLuckHint;
    [SerializeField] GameObject congratsHint;
    [SerializeField] GameObject tileParent;
    [SerializeField] GameObject stateSwitchButton;
    [SerializeField] GameObject OverlayMenu;
    Vector3 ballStartPosition;

     enum TutorialState {
        Start, ChangeState, Shooting, GoodLuck, ReachGoal,
    }
    [SerializeField]
    TutorialState tutorialState;

    void Start()
    {
        tutorialState = TutorialState.Start;
    }

    void Update()
    {
        if(tutorialState == TutorialState.Start){
            bool tile1_rotated = Mathf.Approximately(tileParent.transform.Find("Tile").eulerAngles.z, 0);
            bool tile2_rotated = Mathf.Approximately(tileParent.transform.Find("Tile (2)").eulerAngles.z, 180);
            if(!tile1_rotated || !tile2_rotated){
                changeStateHint.SetActive(true);
                stateSwitchButton.SetActive(true);
                tutorialState = TutorialState.ChangeState;
            }
            if(!gameController.isRotateState){
                stateSwitchButton.SetActive(true);
                rotateTileHint.SetActive(false);
                changeStateHint.SetActive(false);
                shootBallHint.SetActive(true);
                ballStartPosition = ball.transform.position;
                tutorialState = TutorialState.Shooting;
            }
        }
        else if(tutorialState == TutorialState.ChangeState){
            if(!gameController.isRotateState){
                rotateTileHint.SetActive(false);
                changeStateHint.SetActive(false);
                shootBallHint.SetActive(true);
                ballStartPosition = ball.transform.position;
                tutorialState = TutorialState.Shooting;
            }
        }
        else if(tutorialState == TutorialState.Shooting){
            if(Vector3.Distance(ball.transform.position, ballStartPosition) > 1){
                shootBallHint.SetActive(false);
                goodLuckHint.SetActive(true);
                for(int i=0;i<OverlayMenu.transform.childCount;i++){
                    OverlayMenu.transform.GetChild(i).gameObject.SetActive(true);
                }
                tutorialState = TutorialState.GoodLuck;
            }
            else if(gameController.isRotateState){
                shootBallHint.SetActive(false);
                rotateTileHint.SetActive(true);
                changeStateHint.SetActive(true);
                tutorialState = TutorialState.ChangeState;
            }
        }
        else if(tutorialState == TutorialState.GoodLuck){
            if(!gameController.isGameState) {
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
