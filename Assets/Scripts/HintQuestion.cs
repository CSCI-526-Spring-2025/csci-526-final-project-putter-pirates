using TMPro;
using UnityEngine;

public class HintQuestion : MonoBehaviour
{
    GameController gc;
    TextMeshProUGUI helpText;
    private GameObject panel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        helpText = GameObject.Find("HelpText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void ShowHint()
    {

    }
}
