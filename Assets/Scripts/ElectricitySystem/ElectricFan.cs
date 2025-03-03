using UnityEngine;

public class ElectricFan : MonoBehaviour
{
    [SerializeField]
    bool powered;
    [SerializeField]
    bool isActivatedWhenPowered;
    GameObject cross;
    ElectricityComponent electricityComponent;
    AreaEffector2D areaEffector2D;

    void Start()
    {
        cross = transform.Find("Cross").gameObject;
        areaEffector2D = gameObject.GetComponent<AreaEffector2D>();

        electricityComponent = transform.Find("ElectricityTrigger").GetComponent<ElectricityComponent>();
    }

    void Update()
    {
        powered = electricityComponent.isCharged;
        bool fanActivated = isActivatedWhenPowered ? powered : !powered;
        cross.SetActive(!fanActivated);
        areaEffector2D.enabled = fanActivated;
    }
}
