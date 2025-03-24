using UnityEngine;

public class LevelUIManagement : MonoBehaviour
{
    GameController gameController;
    LevelLoader levelLoader;

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

    public void ToggleGameState()
    {
        gameController.ToggleState();
    }

    public void ReloadCurrentLevel()
    {
        levelLoader.ReloadCurrentLevel();
    }

    public void LoadNextLevel()
    {
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
}
