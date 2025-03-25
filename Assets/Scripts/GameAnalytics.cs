using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;

public class GameAnalytics : MonoBehaviour
{
    private int levelNum;
    private int shotCount = 0; // Track shots per game
    private string databaseURL;
    public static GameAnalytics instance;

    public TileInitializer tileinitializer;

    void Awake()
    {
        levelNum = SceneManager.GetActiveScene().buildIndex;
        tileinitializer = FindObjectOfType<TileInitializer>();
        databaseURL = $"https://putterdatabase-default-rtdb.firebaseio.com/analytics/level{levelNum}/";
        Debug.Log("🎯 Level " + levelNum);
        if (instance == null)
        {
            instance = this;
        }
    }

    // 🔹 Track each shot during gameplay
    public void TrackShot()
    {
        shotCount++;
        Debug.Log("🎯 Shot Taken! Total Shots: " + shotCount);

    }

//     public void PrintTileStates()
// {
//     Debug.Log("Hii");
//     Debug.Log(tileinitializer.tilesRotationStates);
//     if (tileinitializer == null || tileinitializer.tilesRotationStates == null)
//     {
//         Debug.LogWarning("⚠️ Tileinitializer or tilesRotationStates not assigned!");
//         return;
//     }

//     List<int> tilesRotationStates = tileinitializer.tilesRotationStates;
//     string states = string.Join(", ", tilesRotationStates);
//     Debug.Log("🧩 Tile Rotation States: [" + states + "]");
// }


    // 🔹 Append the final shot count to Firebase at the end of the game
    public void AppendShotData()
    {
        StartCoroutine(AppendShotCount(shotCount));
        shotCount = 0;
    }

    private IEnumerator AppendShotCount(int finalShotCount)
    {
        string postURL = databaseURL + "shots.json";
        string json = "{\"shots\": " + finalShotCount + "}";

        UnityWebRequest postRequest = new UnityWebRequest(postURL, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        postRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        postRequest.downloadHandler = new DownloadHandlerBuffer();
        postRequest.SetRequestHeader("Content-Type", "application/json");

        yield return postRequest.SendWebRequest();

        if (postRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ New shot count appended successfully!");
        }
        else
        {
            Debug.LogError("❌ Error appending shot count: " + postRequest.error);
        }
    }


    public void AppendStateData()
    {
        StartCoroutine(AppendAllStates());
    }

private IEnumerator AppendAllStates()
{
    if (tileinitializer == null || tileinitializer.tilesRotationStates == null)
    {
        Debug.LogWarning("⚠️ Tileinitializer or tilesRotationStates not assigned!");
        yield break;
    }

    List<int> tilesRotationStates = tileinitializer.tilesRotationStates;
    string states = string.Join(", ", tilesRotationStates);
    Debug.Log("🧩 Tile Rotation States: [" + states + "]");

    string postURL = databaseURL + "state.json";

    // Proper JSON array formatting
    string json = "{\"state\":[" + states + "]}";

    UnityWebRequest postRequest = new UnityWebRequest(postURL, "POST");
    byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
    postRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
    postRequest.downloadHandler = new DownloadHandlerBuffer();
    postRequest.SetRequestHeader("Content-Type", "application/json");

    yield return postRequest.SendWebRequest();

    if (postRequest.result == UnityWebRequest.Result.Success)
    {
        Debug.Log("✅ Tile rotation states appended successfully!");
    }
    else
    {
        Debug.LogError("❌ Error appending rotation states: " + postRequest.error);
    }
}

}

// 🔹 Helper class for JSON conversion (optional)
[System.Serializable]
public class FirebaseShotData
{
    public List<int> shots;

    public FirebaseShotData(List<int> shots)
    {
        this.shots = shots;
    }
}