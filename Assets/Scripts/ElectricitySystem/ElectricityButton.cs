using TMPro;
using UnityEngine;

public class ElectricityButton : MonoBehaviour
{
    public GameObject unpressedCap;
    public GameObject pressedCap;
    public bool pressed = false;
    ElectricityComponent electricityComponent;
    ElectricityManager electricityManager;

    void Start()
    {
        electricityManager = GameObject.Find("ElectricityManager").GetComponent<ElectricityManager>();
        electricityComponent = transform.Find("ElectricityTrigger").GetComponent<ElectricityComponent>();
        electricityComponent.isWire = false;
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
            electricityManager.RegisterElectricitySource(electricityComponent);
            electricityComponent.isCharged = true;
            electricityComponent.isEndPoint = false;
            electricityComponent.isSource = true;
            Debug.Log("Button Pressed!");
        }
    }

    public void ResetButton()
    {
        pressed = false;
        unpressedCap.SetActive(!pressed);
        pressedCap.SetActive(pressed);

        electricityComponent.isCharged = false;
        electricityComponent.isEndPoint = true;
        electricityComponent.isSource = false;
        electricityManager.RemoveElectricitySource(electricityComponent);
    }
}
