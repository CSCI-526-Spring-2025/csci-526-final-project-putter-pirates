using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FanSpinning : MonoBehaviour
{
    public GameObject target;
    public GameObject[] triangles;
    public Vector3 pivot = Vector3.zero;
    public Vector3 rotationSpeed = new Vector3(0, 0, 30.0f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, 5.0f * Time.deltaTime, 0.0f);

    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        return dir + pivot;
    }
}

