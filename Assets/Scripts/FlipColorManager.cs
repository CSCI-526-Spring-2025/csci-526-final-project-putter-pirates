using UnityEngine;

public class FlipColorManager : MonoBehaviour
{

    private Color32 grassRotate = new Color32(222, 162, 74, 255); // #DEA24A
    // private Color32 grassPlay = new Color32(177, 194, 78, 255); // #B1C262
    private Color32 grassPlay = new Color32(150, 194, 70, 255); // #B1C262


    private Color32 boundaryRotate = new Color32(220, 206, 157, 255); // #DCCE9D
    private Color32 backgroundPlay = new Color32(120, 170, 240, 255); // not -> #BADDFB

    private Color32 backgroundRotate = new Color32(176, 164, 109, 255); // #B0A46D
    private Color32 boundaryPlay = new Color32(186, 221, 251, 255); // #BADDFB;

    public GameObject[] grass;
    public GameObject[] tileBackgrounds;
    public GameObject[] tileBoundaries;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grass = GameObject.FindGameObjectsWithTag("Grass");
        tileBackgrounds = GameObject.FindGameObjectsWithTag("TileBackGround");
        tileBoundaries = GameObject.FindGameObjectsWithTag("TileBoundary");

        foreach (GameObject grassPatch in grass)
        {
            grassPatch.GetComponent<SpriteRenderer>().color = grassRotate;
        }

        foreach (GameObject background in tileBackgrounds)
        {
            background.GetComponent<SpriteRenderer>().color = backgroundRotate;
        }

        foreach (GameObject boundary in tileBoundaries)
        {
            boundary.GetComponent<SpriteRenderer>().color = boundaryRotate;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flip(bool isRotateState)
    {
        FlipWalls(isRotateState);
        FlipBackgrounds(isRotateState);
        FlipBoundaries(isRotateState);
    }

    void FlipWalls(bool isRotateState)
    {
        foreach (GameObject grassPatch in grass)
        {
            if (isRotateState)
            {
                grassPatch.GetComponent<SpriteRenderer>().color = grassRotate;
            } else
            {
                grassPatch.GetComponent<SpriteRenderer>().color = grassPlay;
            }
            
        }
    }

    void FlipBackgrounds(bool isRotateState)
    {
        foreach (GameObject background in tileBackgrounds)
        {
            if (isRotateState)
            {
                background.GetComponent<SpriteRenderer>().color = backgroundRotate;
            } else
            {
                background.GetComponent<SpriteRenderer>().color = backgroundPlay;
            }
            
        }
    }

    void FlipBoundaries(bool isRotateState)
    {
        foreach (GameObject boundary in tileBoundaries)
        {
            if (isRotateState)
            {
                boundary.GetComponent<SpriteRenderer>().color = boundaryRotate;
            }
            else
            {
                boundary.GetComponent<SpriteRenderer>().color = boundaryPlay;
            }

        }
    }
}
