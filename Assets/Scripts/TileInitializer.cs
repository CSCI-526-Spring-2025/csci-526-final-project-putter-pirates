using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TileInitializer : MonoBehaviour
{
    public GameObject tileParent;

    void Start()
{
    InitializeTileIndices(); // for when the game starts in this scene
}

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
        // InitializeTileIndices();
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
            Debug.Log("Tilescript : " + tileScript.index);
        }

        Debug.Log($"Tile indices initialized for level: {SceneManager.GetActiveScene().name}");
    }

public List<int> tilesRotationStates = new List<int>();

public void PrintTilesRotationState()
{
    if (tileParent == null)
    {
        Debug.LogError("Tile parent is not assigned!");
        return;
    }

    // Find max index to correctly size the list
    int maxIndex = 0;
    foreach (Transform tile in tileParent.transform)
    {
        var script = tile.GetComponent<Tile>();
        if (script != null && script.index > maxIndex)
            maxIndex = script.index;
    }

    // Initialize the list with a default value (-1 indicates undefined)
    tilesRotationStates = new List<int>(new int[maxIndex + 1]);

    foreach (Transform tile in tileParent.transform)
    {   
        var tileScript = tile.GetComponent<Tile>();
        if (tileScript != null)
        {
            float rotation = tileScript.rotation;
            float zRotation = Mathf.Abs(tile.transform.eulerAngles.z - rotation);
            int state;

            if (Mathf.Approximately(zRotation, 0))
                state = 0;
            else if (Mathf.Approximately(zRotation, 90))
                state = 1;
            else if (Mathf.Approximately(zRotation, 180))
                state = 2;
            else if (Mathf.Approximately(zRotation, 270))
                state = 3;
            else
                state = -1; // -1 indicates undefined or unexpected rotation

            // Store state at the index corresponding to tileScript.index
            tilesRotationStates[tileScript.index] = state;

            // Logging for debug purposes
            // Debug.Log($"Tile {tileScript.index}: {state}");
        }
    }

}
}