using UnityEngine;

public class ElectricityButton : MonoBehaviour
{
    public GameObject unpressedCap;
    public GameObject pressedCap;
    public bool pressed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetButton();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space)) ResetButton();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !pressed)
        {
            pressed = true;
            unpressedCap.SetActive(!pressed);
            pressedCap.SetActive(pressed);
            Debug.Log("Button Pressed!");
        }
    }

    public void ResetButton()
    {
        pressed = false;
        unpressedCap.SetActive(!pressed);
        pressedCap.SetActive(pressed);
    }
}
