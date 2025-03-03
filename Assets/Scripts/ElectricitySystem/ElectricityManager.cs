using System.Collections.Generic;
using UnityEngine;

public class ElectricityManager : MonoBehaviour
{
    [SerializeField]
    List<ElectricityComponent> ElectricityComponents;
    [SerializeField]
    List<ElectricityComponent> ElectricitySources;
    bool refreshmentScheduled;

    void Start()
    {
        refreshmentScheduled = true;
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
        ScheduleElectricityRefreshment();
    }

    public void RegisterElectricitySource(ElectricityComponent ec)
    {
        ElectricitySources.Add(ec);
        ScheduleElectricityRefreshment();
    }

    public void RemoveElectricitySource(ElectricityComponent ec)
    {
        if(!ElectricitySources.Contains(ec)) return;
        ElectricitySources.Remove(ec);
        ScheduleElectricityRefreshment();
    }

    public void ScheduleElectricityRefreshment()
    {
        refreshmentScheduled = true;
    }
}
