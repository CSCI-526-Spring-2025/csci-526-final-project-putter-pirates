using System.Collections.Generic;
using UnityEngine;

public class ElectricityComponent : MonoBehaviour
{
    public bool isCharged = false;
    public bool isEndPoint = false;
    public bool isWire = false;
    public bool isSource = false;
    [SerializeField]
    List<ElectricityComponent> connectedElectricityComponents;

    ElectricityManager ElectricityManager;

    void Start()
    {
        ElectricityManager = GameObject.Find("ElectricityManager").GetComponent<ElectricityManager>();
        if(isSource) ElectricityManager.RegisterElectricitySource(this);
        else ElectricityManager.RegisterElectricityComponent(this);
        ElectricityManager.ScheduleElectricityRefreshment();
    }

    void Update()
    {
        if (isWire)
        {
            Color unpoweredColor = new Color(0.57f, 0.6f, 0);
            Color poweredColor = new Color(0.9f, 1, 0);
            gameObject.GetComponent<SpriteRenderer>().color = isCharged ? poweredColor : unpoweredColor;
        }
    }

    public void chargeAndPropagate()
    {
        if (isCharged) return;

        isCharged = true;
        if (isEndPoint) return;
        foreach (ElectricityComponent ec in connectedElectricityComponents)
        {
            ec.chargeAndPropagate();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ElectricityComponent otherEC = other.gameObject.GetComponent<ElectricityComponent>();
        if (otherEC == null || connectedElectricityComponents.Contains(otherEC)) return;
        connectedElectricityComponents.Add(otherEC);
        ElectricityManager.ScheduleElectricityRefreshment();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ElectricityComponent otherEC = other.gameObject.GetComponent<ElectricityComponent>();
        if (otherEC == null || !connectedElectricityComponents.Contains(otherEC)) return;
        connectedElectricityComponents.Remove(otherEC);
        ElectricityManager.ScheduleElectricityRefreshment();
    }
}
