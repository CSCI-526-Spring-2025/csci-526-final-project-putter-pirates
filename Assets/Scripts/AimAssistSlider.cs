using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AimAssistSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI label;

    GameAnalytics gameAnalytics;

    void Awake()
    {
        // Load saved slider value when the scene starts
        slider.value = PlayerPrefs.GetFloat("AimAssistValue", 10);
        label.text = slider.value.ToString();
        gameAnalytics = FindObjectOfType<GameAnalytics>();


        // Subscribe to scene change events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        slider.onValueChanged.AddListener((v) =>
        {
            label.text = v.ToString();
            PlayerPrefs.SetFloat("AimAssistValue", v);
            if(gameAnalytics) gameAnalytics.SetAim(v);
            PlayerPrefs.Save(); // Save persistently
        });
    }

    // Ensure slider updates correctly when a new level loads
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        slider.value = PlayerPrefs.GetFloat("AimAssistValue", 10);
        gameAnalytics.SetAim(10); //change whenever default slider value is changed
        label.text = slider.value.ToString();
    }

    void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
