using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2)) LoadLevel2();

        if (Input.GetKeyDown(KeyCode.Alpha1)) LoadLevel1();

        if (Input.GetKeyDown(KeyCode.Alpha0)) LoadLevel0();
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
}
