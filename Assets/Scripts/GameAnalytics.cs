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
    // private int skipped = 0;
    private string databaseURL;
    public static GameAnalytics instance;
    private float aim;
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

    public void SetAim(float var){
        this.aim = var;
        Debug.Log("Aim : " + aim);
    }

    //  public void TrackSkipped()
    // {
    //     StartCoroutine(IncrementSkipCount());

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
    public void AppendAimData()
    {
        StartCoroutine(AppendAimAssist());
    }
private IEnumerator AppendAimAssist()
{
        string postURL = databaseURL + "aim.json";
        string json = "{\"aim\": " + aim + "}";

        UnityWebRequest postRequest = new UnityWebRequest(postURL, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        postRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        postRequest.downloadHandler = new DownloadHandlerBuffer();
        postRequest.SetRequestHeader("Content-Type", "application/json");

        yield return postRequest.SendWebRequest();

        if (postRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ New aim assit appended successfully!");
        }
        else
        {
            Debug.LogError("❌ Error appending shot count: " + postRequest.error);
        }
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
        // Debug.Log("✅ Tile rotation states appended successfully!");
    }
    else
    {
        Debug.LogError("❌ Error appending rotation states: " + postRequest.error);
    }
}

// private IEnumerator IncrementSkipCount()
// {
//     string getURL = databaseURL + "skip.json";
//     Debug.Log($"Request URL: {getURL}");

//     UnityWebRequest getRequest = UnityWebRequest.Get(getURL);
//     var operation = getRequest.SendWebRequest();

//     // Use callback completion instead of yield directly
//     bool isDone = false;
//     operation.completed += (_) => isDone = true;

//     while (!isDone)
//     {
//         yield return null;
//     }

//     if (getRequest.result != UnityWebRequest.Result.Success)
//     {
//         Debug.LogError("❌ Error fetching skip count: " + getRequest.error);
//         yield break;
//     }

//     int currentSkipCount = 0;
//     string resultText = getRequest.downloadHandler.text.Trim();
//     Debug.Log($"Firebase response: '{resultText}'");

//     if (resultText != "null" && int.TryParse(resultText, out currentSkipCount))
//     {
//         currentSkipCount += 1;
//         Debug.Log($"Incremented SkipCount to {currentSkipCount}");
//     }
//     else
//     {
//         currentSkipCount = 0;
//         Debug.Log($"Initialized SkipCount to {currentSkipCount}");
//     }

//     // Update the value back
//     string json = currentSkipCount.ToString();
//     UnityWebRequest putRequest = UnityWebRequest.Put(getURL, json);
//     putRequest.SetRequestHeader("Content-Type", "application/json");

//     var putOperation = putRequest.SendWebRequest();
//     isDone = false;
//     putOperation.completed += (_) => isDone = true;

//     while (!isDone)
//     {
//         yield return null;
//     }

//     if (putRequest.result == UnityWebRequest.Result.Success)
//     {
//         Debug.Log("✅ Skip count updated successfully to: " + currentSkipCount);
//     }
//     else
//     {
//         Debug.LogError("❌ Error updating skip count: " + putRequest.error);
//     }
// }





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