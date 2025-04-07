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
    private float aim = 10;
    private int helpCount = 1;
    public TileInitializer tileinitializer;

    void Awake()
    {
        levelNum = SceneManager.GetActiveScene().buildIndex - 1;
        if(levelNum == 10){
            // update when more levels are added
            levelNum = 0;
        }
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

    public void OnHelpButtonClicked()
    {
        StartCoroutine(IncrementHelpCount());
    }

    // 🔹 Append the final shot count to Firebase at the end of the game
    public void AppendShotData()
    {
        StartCoroutine(AppendShotCount(shotCount));
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
        // Debug.Log("Tile rotation states appended successfully!");
    }
    else
    {
        Debug.LogError("Error appending rotation states: " + postRequest.error);
    }
}

private IEnumerator IncrementHelpCount()
{
    string getURL = databaseURL + "help.json";
    Debug.Log($"Request URL: {getURL}");

    UnityWebRequest getRequest = UnityWebRequest.Get(getURL);
    var operation = getRequest.SendWebRequest();

    bool isDone = false;
    operation.completed += (_) => isDone = true;

    while (!isDone)
    {
        yield return null;
    }

    if (getRequest.result != UnityWebRequest.Result.Success)
    {
        Debug.LogError("Error fetching help count: " + getRequest.error);
        yield break;
    }

    int currentHelpCount= 0;
    string resultText = getRequest.downloadHandler.text.Trim();
    Debug.Log($"Firebase response: '{resultText}'");

    if (resultText != "null" && int.TryParse(resultText, out currentHelpCount))
    {
        currentHelpCount += 1;
        Debug.Log($"Incremented HelpCount to {currentHelpCount}");
    }
    else
    {
        currentHelpCount = 0;
        Debug.Log($"Initialized HelpCount to {currentHelpCount}");
    }

    string json = currentHelpCount.ToString();
    UnityWebRequest putRequest = UnityWebRequest.Put(getURL, json);
    putRequest.SetRequestHeader("Content-Type", "application/json");

    var putOperation = putRequest.SendWebRequest();
    isDone = false;
    putOperation.completed += (_) => isDone = true;

    while (!isDone)
    {
        yield return null;
    }

    if (putRequest.result == UnityWebRequest.Result.Success)
    {
        Debug.Log("Help count updated successfully to: " + currentHelpCount);
    }
    else
    {
        Debug.LogError("Error updating help count: " + putRequest.error);
    }
}


public void TrackGoalReached()
{
    StartCoroutine(AppendCompletionData());
    shotCount = 0;

}

private IEnumerator AppendCompletionData()
{
    if (tileinitializer == null || tileinitializer.tilesRotationStates == null)
    {
        Debug.LogWarning("Tileinitializer or tilesRotationStates not assigned!");
        yield break;
    }

    List<int> tilesRotationStates = tileinitializer.tilesRotationStates;
    string states = string.Join(", ", tilesRotationStates);
    Debug.Log("🏁 Goal Reached - State: [" + states + "], Shots: " + shotCount);

    string postURL = databaseURL + "completed.json";

    // Construct JSON with both state and shots
    string json = $"{{\"state\":[{states}], \"shots\":{shotCount}}}";

    UnityWebRequest postRequest = new UnityWebRequest(postURL, "POST");
    byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
    postRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
    postRequest.downloadHandler = new DownloadHandlerBuffer();
    postRequest.SetRequestHeader("Content-Type", "application/json");

    yield return postRequest.SendWebRequest();

    if (postRequest.result == UnityWebRequest.Result.Success)
    {
        Debug.Log("Goal data appended successfully!");
    }
    else
    {
        Debug.LogError("Error appending goal data: " + postRequest.error);
    }

    // Optionally reset shot count if needed
    shotCount = 0;
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