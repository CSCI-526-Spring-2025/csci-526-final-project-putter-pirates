using System.Collections.Generic;
using UnityEngine;

public class ElectricityManager : MonoBehaviour
{
    [SerializeField]
    List<ElectricityComponent> electricityComponents;
    [SerializeField]
    List<ElectricityComponent> electricitySources;
    [SerializeField]
    List<ElectricityButton> electricityButtons;
    bool refreshmentScheduled;

    void Start()
    {
        refreshmentScheduled = true;
    }

    void LateUpdate()
    {
        if(refreshmentScheduled)
        {
            foreach(ElectricityComponent ec in electricityComponents) ec.isCharged = false;
            foreach(ElectricityComponent ec in electricitySources) ec.isCharged = false;
            foreach(ElectricityComponent ec in electricitySources) ec.chargeAndPropagate();
            refreshmentScheduled = false;
        }
    }

    public void RegisterElectricityComponent(ElectricityComponent ec)
    {
        electricityComponents.Add(ec);
        ScheduleElectricityRefreshment();
    }

    public void RegisterElectricitySource(ElectricityComponent ec)
    {
        electricitySources.Add(ec);
        ScheduleElectricityRefreshment();
    }

    public void RemoveElectricitySource(ElectricityComponent ec)
    {
        if(!electricitySources.Contains(ec)) return;
        electricitySources.Remove(ec);
        ScheduleElectricityRefreshment();
    }

    public void RegisterElectricityButton(ElectricityButton eb)
    {
        electricityButtons.Add(eb);
        // ScheduleElectricityRefreshment();
    }

    public void ScheduleElectricityRefreshment()
    {
        refreshmentScheduled = true;
    }

    public void ResetButtons()
    {
        foreach(ElectricityButton eb in electricityButtons)
        {
            eb.ResetButton();
        }
    }
}
