using UnityEngine;
using UnityEngine.SceneManagement;

public class TileInitializer : MonoBehaviour
{
    public GameObject tileParent;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeTileIndices();
    }

    void InitializeTileIndices()
    {
        if (tileParent == null)
        {
            Debug.LogError("Tile parent is not assigned!");
            return;
        }

        int index = 0;
        foreach (Transform tile in tileParent.transform)
        {
            var tileScript = tile.GetComponent<Tile>();
            if (tileScript != null)
            {
                tileScript.index = index++;
            }
        }

        Debug.Log($"Tile indices initialized for level: {SceneManager.GetActiveScene().name}");
    }
}
