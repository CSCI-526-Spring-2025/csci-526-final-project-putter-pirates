using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int levelNum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string name = SceneManager.GetActiveScene().name;
        levelNum = name[name.Length - 1] - '0';
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5)) LoadLevel(9);

        if (Input.GetKeyDown(KeyCode.Alpha5)) LoadLevel(8);
        
        if (Input.GetKeyDown(KeyCode.Alpha5)) LoadLevel(7);
        
        if (Input.GetKeyDown(KeyCode.Alpha5)) LoadLevel(6);
        
        if (Input.GetKeyDown(KeyCode.Alpha5)) LoadLevel(5);

        if (Input.GetKeyDown(KeyCode.Alpha4)) LoadLevel(4);

        if (Input.GetKeyDown(KeyCode.Alpha3)) LoadLevel(3);

        if (Input.GetKeyDown(KeyCode.Alpha2)) LoadLevel(2);

        if (Input.GetKeyDown(KeyCode.Alpha1)) LoadLevel(1);

        if (Input.GetKeyDown(KeyCode.Alpha0)) LoadLevel(0);

        if (Input.GetKeyDown(KeyCode.W)) CompleteResetLevel();

    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevel0()
    {
        SceneManager.LoadScene("Level0");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void LoadLevel(int level)
    {
        levelNum = level;
        string name = "Level" + level.ToString();
        SceneManager.LoadScene(name);
    }

    public void LoadNext()
    {
        levelNum++;
        string name = "Level" + levelNum.ToString();
        SceneManager.LoadScene(name);
        
    }

    public void LoadPrev()
    {
        levelNum--;
        string name = "Level" + levelNum.ToString();
        SceneManager.LoadScene(name);

    }

    // move tiles back to starting orientation
    public void CompleteResetLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
