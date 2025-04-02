using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Trajectory : MonoBehaviour
{
    public int numDots = 10;
    private int maxDots = 30;
    public GameObject dotsParent;
    public GameObject dotPrefab;
    public float dotSpacing;

    Transform[] dotsList;
    Vector2 pos;
    float timeGap;
    private GameController gc;
    [SerializeField] private Slider slider;

    void Awake()
    {
        // Subscribe to scene load event to keep numDots across levels
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();

        Hide();
        PrepareDots();

        // Load numDots from PlayerPrefs across all levels
        numDots = PlayerPrefs.GetInt("numDots", 10); // Default to 10 if not set
        slider.value = numDots;

        slider.onValueChanged.AddListener((v) =>
        {
            numDots = (int)v;
            PlayerPrefs.SetInt("numDots", numDots);
            PlayerPrefs.Save(); // Save persistently
        });
    }

    // This ensures numDots is correctly set when a new level loads
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        numDots = PlayerPrefs.GetInt("numDots", 10);
        slider.value = numDots;
        UpdateDots(Vector3.zero, Vector2.zero); // Reset dots if needed
    }

    void PrepareDots()
    {
        dotsList = new Transform[maxDots];

        for (int i = 0; i < maxDots; i++)
        {
            dotsList[i] = Instantiate(dotPrefab, null).transform;
            dotsList[i].parent = dotsParent.transform;
        }
    }

    public void UpdateDots(Vector3 ballPos, Vector2 initialVelocity)
    {
        if (gc.isPaused || initialVelocity.Equals(Vector2.zero)) return;

        timeGap = dotSpacing;
        for (int i = 0; i < numDots; i++)
        {
            pos.x = ballPos.x + initialVelocity.x * timeGap;
            pos.y = ballPos.y + (initialVelocity.y * timeGap) - (Physics2D.gravity.magnitude * 1.5f * timeGap * timeGap) / 2f;

            dotsList[i].position = pos;
            timeGap += dotSpacing;
        }

        for (int i = numDots; i < maxDots; i++)
        {
            dotsList[i].position = ballPos;
        }
    }

    public void Show()
    {
        if (!gc.isPaused)
        {
            dotsParent.SetActive(true);
            Debug.Log("numDots: " + numDots);
        }
    }

    public void Hide()
    {
        dotsParent.SetActive(false);
    }

    void OnDestroy()
    {
        // Unsubscribe from the scene load event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}



    //public void UpdateDots(Vector3 ballPos, Vector2 forceApplied)
    //{
    //    timeGap = dotSpacing;
    //    Vector2 acceleration = forceApplied / 10;
    //    for (int i = 0; i < numDots; i++)
    //    {
    //        pos.x = ballPos.x + (acceleration.x * timeGap * timeGap)/2f;
    //        pos.y = ballPos.y + (acceleration.y * timeGap * timeGap)/2f - (Physics2D.gravity.magnitude * 1.5f * timeGap * timeGap) / 2f;

    //        dotsList[i].position = pos;

    //        timeGap += dotSpacing;
    //    }
    //}

