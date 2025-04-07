using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public const int level0Index = 1;
    public int levelNum;
    public bool isLevel0;
    public bool isLastLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        levelNum = SceneManager.GetActiveScene().buildIndex - level0Index;
        isLevel0 = levelNum == 0;
        isLastLevel = SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings-1;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0;i<10;i++)
        {
            if (Input.GetKeyDown(i.ToString())) LoadLevel(level0Index+i);
        }

        if (Input.GetKeyDown(KeyCode.W)) ReloadCurrentLevel();

    }

    public void LoadLevel(int level, bool withOffset = true)
    {
        if(withOffset) SceneManager.LoadScene(level + level0Index);
        else SceneManager.LoadScene(level);
    }

    public void LoadLevelButton(int level)
    {
        LoadLevel(level);    
    }

    public void LoadNext()
    {
        if(isLastLevel) return;
        LoadLevel(levelNum+1);
    }

    public void LoadPrev()
    {
        if(isLevel0) return;
        LoadLevel(levelNum-1);
    }

    // move tiles back to starting orientation
    public void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextDelayed()
    {
        StartCoroutine(DelayedSceneChange());
    }
    
    private IEnumerator DelayedSceneChange()
    {
        yield return new WaitForSeconds(10); // Wait for 10 seconds
        LoadNext();
    }
}
