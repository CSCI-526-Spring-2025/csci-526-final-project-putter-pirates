using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GameAnalytics : MonoBehaviour
{
    private string databaseURL = "https://putterdatabase-default-rtdb.firebaseio.com/analytics/shots.json"; // Replace with your Firebase URL
    private int shotCount = 0; // Track shots per game

    public static GameAnalytics instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // *🔹 Track each shot during gameplay*
    public void TrackShot()
    {
        shotCount++;  // Increase shot count when ball is shot
        Debug.Log("🎯 Shot Taken! Total Shots: " + shotCount);
    }

    // *🔹 Append the final shot count to Firebase at the end of the game*
    public void AppendShotData()
    {
        StartCoroutine(AppendShotCount(shotCount));
        shotCount = 0;  // Reset counter for next game
    }

private IEnumerator AppendShotCount(int finalShotCount)
{
    // 🔹 Instead of retrieving data, send a new entry using "POST"
    string json = "{\"shots\": " + finalShotCount + "}";

    UnityWebRequest postRequest = new UnityWebRequest(databaseURL, "POST");
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
}

// *🔹 Helper class for JSON conversion*
[System.Serializable]
public class FirebaseShotData
{
    public List<int> shots;

    public FirebaseShotData(List<int> shots)
    {
        this.shots = shots;
    }
}