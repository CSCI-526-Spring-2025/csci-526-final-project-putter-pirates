using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AimAssistSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI label;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.value = 10;
        slider.onValueChanged.AddListener((v) =>
        {
            label.text = v.ToString();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
