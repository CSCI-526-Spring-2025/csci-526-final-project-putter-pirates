using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public int numDots;
    public GameObject dotsParent;
    public GameObject dotPrefab;
    public float dotSpacing;

    Transform[] dotsList;

    Vector2 pos;
    float timeGap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Hide();
        PrepareDots();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PrepareDots()
    {
        dotsList = new Transform[numDots];

        for (int i = 0; i < numDots; i++)
        {
            dotsList[i] = Instantiate(dotPrefab, null).transform;
            dotsList[i].parent = dotsParent.transform;
        }
    }

    public void UpdateDots(Vector3 ballPos, Vector2 initialVelocity)
    {
        timeGap = dotSpacing;
        for (int i = 0; i < numDots; i++)
        {
            pos.x = ballPos.x + initialVelocity.x * timeGap;
            pos.y = ballPos.y + (initialVelocity.y * timeGap) - (Physics2D.gravity.magnitude * 1.5f * timeGap * timeGap) / 2f;

            dotsList[i].position = pos;

            timeGap += dotSpacing;
        }
    }

    //public void UpdateDots(Vector3 ballPos, Vector2 forceApplied)
    //{
    //    timeGap = dotSpacing;
    //    Vector2 acceleration = forceApplied / 10;
    //    for (int i = 0; i < numDots; i++)
    //    {
    //        pos.x = ballPos.x + (acceleration.x * timeGap * timeGap)/2f;
    //        pos.y = ballPos.y + (acceleration.y * timeGap * timeGap)/2f - (Physics2D.gravity.magnitude * 1.5f * timeGap * timeGap) / 2f;

    //        dotsList[i].position = pos;

    //        timeGap += dotSpacing;
    //    }
    //}

    public void Show()
    {
        dotsParent.SetActive(true);

    }

    public void Hide()
    {
        dotsParent.SetActive(false);
    }
}
