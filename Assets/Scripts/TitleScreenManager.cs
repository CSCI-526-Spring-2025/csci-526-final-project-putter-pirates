using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] int defualtNumDot = 10;

    void Awake()
    {
        PlayerPrefs.SetInt("numDots", defualtNumDot);
        PlayerPrefs.SetInt("AimAssistValue", defualtNumDot);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
