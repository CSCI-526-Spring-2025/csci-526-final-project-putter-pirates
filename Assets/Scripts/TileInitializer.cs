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

    public void PrintTilesRotationState()
    {
        if (tileParent == null)
        {
            Debug.LogError("Tile parent is not assigned!");
            return;
        }

        foreach (Transform tile in tileParent.transform)
        {
            var tileScript = tile.GetComponent<Tile>();
            float rotation = tileScript.rotation;
            if (tileScript != null)
            {
                float zRotation = Mathf.Abs(tile.transform.eulerAngles.z - rotation);
                string state = "";

                if (Mathf.Approximately(zRotation, 0))
                    state = "0";
                else if (Mathf.Approximately(zRotation, 90))
                    state = "1";
                else if (Mathf.Approximately(zRotation, 180))
                    state = "2";
                else if (Mathf.Approximately(zRotation, 270))
                    state = "3";
                else
                    state = $"other ({zRotation}°)";

                Debug.Log($"Tile {tileScript.index}: {state}");
            }
        }
    }
}
