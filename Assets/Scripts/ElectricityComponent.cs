using System.Collections.Generic;
using UnityEngine;

public class ElectricityComponent : MonoBehaviour
{
    public bool isCharged = false;
    public bool isEndPoint = false;
    public bool isWire = false;
    public 
    List<ElectricityComponent> connectedElectricityComponents;
    ElectricityManager ElectricityManager;

    void Start()
    {
        ElectricityManager = GameObject.Find("ElectricityManager").GetComponent<ElectricityManager>();
        ElectricityManager.RegisterElectricityComponent(this);
    }

    void Update()
    {

    }

    public void chargeAndPropagate()
    {
        if(isEndPoint) return;
        if(isCharged) return;
        isCharged = true;
        foreach(ElectricityComponent ec in connectedElectricityComponents) {
            ec.chargeAndPropagate();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ElectricityComponent otherEC;
        try
        {
            otherEC = other.gameObject.GetComponent<ElectricityComponent>();
        }
        catch (System.Exception)
        {
            Debug.LogWarning(other.gameObject.ToString() + " doesn't have ElectricityComponent");
            return;
        }
        if(connectedElectricityComponents.Contains(otherEC)) return;
        connectedElectricityComponents.Add(otherEC);

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ElectricityComponent otherEC;
        try
        {
            otherEC = other.gameObject.GetComponent<ElectricityComponent>();
        }
        catch (System.Exception)
        {
            Debug.LogWarning(other.gameObject.ToString() + " doesn't have ElectricityComponent");
            return;
        }
        if(!connectedElectricityComponents.Contains(otherEC)) return;
        connectedElectricityComponents.Remove(otherEC);
    }
}
