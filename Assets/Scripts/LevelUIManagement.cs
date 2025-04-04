using System.Collections;
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

    public void LoadNextLevel(bool afterLevelCleared=false)
    {
        if(afterLevelCleared){} //not skipped
        else {} // skipped
        // GameAnalytics.instance.TrackSkipped();
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
        transform.Find("OverlayMenu/MenuButton").gameObject.GetComponent<Button>().interactable = false;
        transform.Find("OverlayMenu/ResetButton").gameObject.GetComponent<Button>().interactable = false;
        transform.Find("OverlayMenu/NextButton").gameObject.GetComponent<Button>().interactable = false;
        transform.Find("OverlayMenu/PrevButton").gameObject.GetComponent<Button>().interactable = false;
        transform.Find("OverlayMenu/StateSwitchButton").gameObject.GetComponent<Button>().interactable = false;
        StartCoroutine(DelayedShowLevelClearPanel());
    }

    IEnumerator DelayedShowLevelClearPanel()
    {
        yield return new WaitForSeconds(2f);
        transform.Find("LevelClearPanel").gameObject.SetActive(true);
    }
}
