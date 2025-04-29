using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManagement : MonoBehaviour
{
    GameController gameController;
    LevelLoader levelLoader;
    public static GameAnalytics instance;

    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        
        transform.Find("OverlayMenu/LevelNumText").GetComponent<TextMeshProUGUI>().text = "Level " + levelLoader.levelNum;

        transform.Find("OverlayMenu/PrevButton").GetComponent<Button>().interactable = !levelLoader.isLevel0;
        transform.Find("LevelClearPanel/PrevButton").GetComponent<Button>().interactable = !levelLoader.isLevel0;
        transform.Find("OverlayMenu/NextButton").GetComponent<Button>().interactable = !levelLoader.isLastLevel;
        transform.Find("LevelClearPanel/NextButton").GetComponent<Button>().interactable = !levelLoader.isLastLevel;
    }

    void Update()
    {
        
    }

    public void ToggleGamePause()
    {
        gameController.TogglePause();
    }

    public void SetGamePause(bool paused)
    {
        gameController.isPaused = paused;
    }

    public void ToggleGameState()
    {
        gameController.ToggleState();
    }

    public void ReloadCurrentLevel()
    {
        levelLoader.ReloadCurrentLevel();
    }

    public void ResetBall()
    {
        gameController.ResetLevel();
    }
    
    public void LoadNextLevel(bool afterLevelCleared=false)
    {
        if(afterLevelCleared){} //not skipped
        else {
            GameAnalytics.instance.TrackSkipped();
        } // skipped
        levelLoader.LoadNext();
    }
    public void LoadPreviousLevel()
    {
        levelLoader.LoadPrev();
    }

    public void LoadSpecificLevel(int level)
    {
        levelLoader.LoadLevel(level, true);
    }

    public void StartLevelClearUIRoutine()
    {
        GameObject overlayMenu = transform.Find("OverlayMenu").gameObject;
        foreach (Button btn in overlayMenu.GetComponentsInChildren<Button>()) btn.interactable = false;
        StartCoroutine(DelayedShowLevelClearPanel());
    }

    IEnumerator DelayedShowLevelClearPanel()
    {
        yield return new WaitForSeconds(2f);
        transform.Find("LevelClearPanel").gameObject.SetActive(true);
    }
}
