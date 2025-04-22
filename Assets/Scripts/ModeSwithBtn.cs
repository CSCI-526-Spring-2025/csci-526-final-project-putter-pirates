using UnityEngine;
using TMPro;

public class ModeSwithBtn : MonoBehaviour
{
    GameController gc;
    TextMeshProUGUI textMeshPro;

    // SpriteRenderer background;
    private Color32 rotateModeColor = new Color32(0, 36, 162, 255);   // #0036F1
    private Color32 playModeColor = new Color32(36, 151, 191, 255);   // #2DC4FF

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        // background = GameObject.Find("Background").GetComponent<SpriteRenderer>();
        textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro.text = gc.isRotateState ? "Rotate" : "Play";
        //if (background != null)
        //{
        //    background.color = gc.isRotateState ? rotateModeColor : playModeColor;
        //}
    }
}
