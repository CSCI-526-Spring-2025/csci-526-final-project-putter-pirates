using UnityEngine;

public class FlipColorManager : MonoBehaviour
{

    private Color32 grassRotate = new Color32(222, 162, 74, 255); // #DEA24A
    private Color32 grassPlay = new Color32(177, 194, 78, 255); // #B1C262

    private Color32 backgroundRotate = new Color32(220, 206, 157, 255); // #DCCE9D
    private Color32 backgroundPlay = new Color32(186, 221, 251, 255); // #BADDFB

    public GameObject[] grass;
    private bool rotate = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grass = GameObject.FindGameObjectsWithTag("Grass");
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flip()
    {
        FlipWalls();
    }

    void FlipWalls()
    {
        if (rotate)
        {
            foreach (GameObject grassPatch in grass)
            {
                grassPatch.GetComponent<SpriteRenderer>().color = grassRotate;
            }
        } else
        {
            foreach (GameObject grassPatch in grass)
            {
                grassPatch.GetComponent<SpriteRenderer>().color = grassPlay;
            }
        }
        

    }
}
