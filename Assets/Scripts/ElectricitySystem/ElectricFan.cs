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
    ParticleSystem windParticles; // NEW

    void Start()
    {
        cross = transform.Find("Cross").gameObject;
        areaEffector2D = GetComponent<AreaEffector2D>();
        electricityComponent = transform.Find("ElectricityTrigger").GetComponent<ElectricityComponent>();

        windParticles = GetComponentInChildren<ParticleSystem>(); // Find the particle system
    }

    void Update()
    {
        powered = electricityComponent.isCharged;
        bool fanActivated = isActivatedWhenPowered ? powered : !powered;

        cross.SetActive(!fanActivated);
        areaEffector2D.enabled = fanActivated;

        if (windParticles != null)
        {
            if (cross.activeSelf && windParticles.isPlaying)
            {
                windParticles.Stop();
            }
            else if (!cross.activeSelf && !windParticles.isPlaying)
            {
                windParticles.Play();
            }
        }
    }
}
