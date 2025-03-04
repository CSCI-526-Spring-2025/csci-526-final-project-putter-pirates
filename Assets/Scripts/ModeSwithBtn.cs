using UnityEngine;
using TMPro;

public class ModeSwithBtn : MonoBehaviour
{
    GameController gc;
    TextMeshProUGUI textMeshPro;
    TextMeshProUGUI instructions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();

        instructions = GameObject.Find("Instructions").GetComponent<TextMeshProUGUI>();
    }

        // Update is called once per frame
        void Update()
    {
        textMeshPro.text = gc.isRotateState ? "Rotate" : "Play";

        instructions.text = gc.isRotateState ? "Click a tile to rotate it!" : "Click and drag to aim the ball!";

        
    }
}
