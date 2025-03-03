using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class GameAnalytics : MonoBehaviour
{
    private string databaseURL = "https://putterdatabase-default-rtdb.firebaseio.com/analytics.json"; // Replace with your Firebase Database URL
    private int shotCount = 0; // Track number of shots

    public static GameAnalytics instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Call this function whenever the ball is shot
    public void TrackShot()
    {
        shotCount++;
        Debug.Log("Ball Shot Count: " + shotCount);
        SaveShotData(shotCount);
    }

    // Function to send shot count data to Firebase
    private void SaveShotData(int shotCount)
    {
        StartCoroutine(SendAnalyticsData(shotCount));
    }

    private IEnumerator SendAnalyticsData(int shotCount)
    {
        string json = "{\"shots\":" + shotCount + "}";

        UnityWebRequest request = new UnityWebRequest(databaseURL, "PATCH");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Shot count updated in Firebase: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("❌ Error updating shot count: " + request.error);
        }
    }
}