using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] int defualtNumDot = 10;
    [SerializeField] private Slider slider;

    void Awake()
    {
        PlayerPrefs.SetInt("numDots", defualtNumDot);
        PlayerPrefs.SetInt("AimAssistValue", defualtNumDot);
    }

    void Start()
    {
        slider.onValueChanged.AddListener((v) =>
        {
            PlayerPrefs.SetInt("numDots", (int)v);
            PlayerPrefs.Save(); // Save persistently
        });
    }

    void Update()
    {
        
    }
}
