using UnityEngine;
using System.Collections.Generic;

public class TileInitializer : MonoBehaviour
{
    public GameObject tileParent;

    public List<Tile> lockingOrder = new List<Tile>(); // Manual hint lock order
    private int nextTileToLockIndex = 0;

    public List<int> tilesRotationStates = new List<int>(); // For analytics

    void Start()
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
    }

    public void LockNextTileInOrder()
    {
        if (nextTileToLockIndex >= lockingOrder.Count)
        {
            Debug.Log("✅ All tiles already locked.");
            return;
        }

        Tile tileToLock = lockingOrder[nextTileToLockIndex];

        if (tileToLock != null && !tileToLock.isLocked)
        {
            tileToLock.SetToGoalState();
            tileToLock.LockTile();
            Debug.Log($"🔒 Locked tile {tileToLock.index} at goal rotation.");

            nextTileToLockIndex++;
        }
        else
        {
            Debug.LogWarning("Tile already locked or null!");
        }
    }

    public void PrintTilesRotationState()
    {
        if (tileParent == null)
        {
            Debug.LogError("Tile parent is not assigned!");
            return;
        }

        tilesRotationStates.Clear();

        foreach (Transform tile in tileParent.transform)
        {
            var tileScript = tile.GetComponent<Tile>();
            if (tileScript != null)
            {
                float startRotation = tileScript.rotation;
                float currentRotation = Mathf.Round(tileScript.transform.eulerAngles.z % 360f);

                float diff = (currentRotation - startRotation + 360f) % 360f;
                int state = Mathf.RoundToInt(diff / 90f) % 4;

                tilesRotationStates.Add(state);
            }
        }

        Debug.Log("🧩 Tile Rotation States (correct clicks): [" + string.Join(", ", tilesRotationStates) + "]");
    }
}
