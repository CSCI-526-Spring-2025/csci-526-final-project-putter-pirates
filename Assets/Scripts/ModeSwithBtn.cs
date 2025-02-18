using UnityEngine;
using TMPro;

public class ModeSwithBtn : MonoBehaviour
{
    GameController gc;
    TextMeshProUGUI textMeshPro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro.text = gc.isRotateState ? "Rotate" : "Play";
    }
}
