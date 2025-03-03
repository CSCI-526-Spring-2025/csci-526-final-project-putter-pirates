using System.Collections.Generic;
using UnityEngine;

public class ElectricityManager : MonoBehaviour
{
    List<ElectricityComponent> ElectricityComponents;
    List<ElectricityComponent> ElectricitySources;
    bool refreshmentScheduled = false;

    void Start()
    {

    }

    void LateUpdate()
    {
        if(refreshmentScheduled)
        {
            foreach(ElectricityComponent ec in ElectricityComponents) ec.isCharged = false;
            foreach(ElectricityComponent ec in ElectricitySources) ec.isCharged = false;
            foreach(ElectricityComponent ec in ElectricitySources) ec.chargeAndPropagate();
            refreshmentScheduled = false;
        }
    }

    public void RegisterElectricityComponent(ElectricityComponent ec)
    {
        ElectricityComponents.Add(ec);
    }

    public void ScheduleElectricityRefreshment()
    {
        refreshmentScheduled = true;
    }
}
