using UnityEngine;

public class CenterAlign : MonoBehaviour
{
    void Start()
    {
        if (transform.parent != null)
        {
            transform.localPosition = Vector3.zero; // Centers the object within the parent
        }
    }
}