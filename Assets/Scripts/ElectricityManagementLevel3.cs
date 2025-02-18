using UnityEngine;

public class ElectricityManagementLevel3 : MonoBehaviour
{
    public GameObject[] windT0;
    public GameObject[] windT1;
    public GameObject[] windT3;
    public GameObject[] wireT1;
    public GameObject[] wireT2;
    public GameObject[] wireT3;
    public GameObject[] doorT0;
    public GameObject[] doorT2;
    GameController gameController;
    ElectricityButton electricityButton;
    GameObject tile0;
    GameObject tile1;
    GameObject tile2;
    GameObject tile3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        electricityButton = GameObject.Find("ElectricityButton").GetComponent<ElectricityButton>();
        tile0 = GameObject.Find("Tile (0)");
        tile1 = GameObject.Find("Tile (1)");
        tile2 = GameObject.Find("Tile (2)");
        tile3 = GameObject.Find("Tile (3)");
    }

    // Update is called once per frame
    void Update()
    {
        bool btnPressed = electricityButton.pressed;
        
        float t0r = tile0.transform.rotation.eulerAngles.z;
        float t1r = tile1.transform.rotation.eulerAngles.z;
        float t2r = tile2.transform.rotation.eulerAngles.z;
        float t3r = tile3.transform.rotation.eulerAngles.z;
        // T0
        foreach(GameObject gb in windT0) SetWindPowered(gb, t0r == 0);
        foreach(GameObject gb in doorT0) SetDoorPowered(gb, t0r == 0);

        // T1
        foreach(GameObject gb in windT1) SetWindPowered(gb, btnPressed);
        foreach(GameObject gb in wireT1) SetWirePowered(gb, btnPressed);

        // T3
        bool T1T3Connected = (t1r == 90 && (t3r == 0 || t3r == 90)) || (t1r == 180 && (t3r == 0 || t3r == 90));
        foreach(GameObject gb in windT3) SetWindPowered(gb, !(T1T3Connected && btnPressed));
        foreach(GameObject gb in wireT3) SetWirePowered(gb, T1T3Connected && btnPressed);

        // T2
        SetDoorPowered(doorT2[0], T1T3Connected && t2r==0 && btnPressed && (t3r==90 || t3r==180));
        SetDoorPowered(doorT2[1], T1T3Connected && t2r==0 && btnPressed && t3r==0);
        foreach(GameObject gb in wireT2) SetWirePowered(gb, T1T3Connected && t2r==0 && btnPressed && t3r==0);
    }

    private void SetWindPowered(GameObject wind, bool powered)
    {
        GameObject cross = wind.transform.Find("Cross").gameObject;
        if(!cross.activeSelf == powered) return;
        if(powered) {
            cross.SetActive(false);
            AreaEffector2D ae = wind.GetComponent<AreaEffector2D>();
            ae.enabled = true;
        }
        else {
            cross.SetActive(true);
            AreaEffector2D ae = wind.GetComponent<AreaEffector2D>();
            ae.enabled = false;
        }
    }

    private void SetDoorPowered(GameObject door, bool powered)
    {
        Color openedColor = new Color(0.368f, 0, 0.671f, 0.5f);
        Color closedColor = new Color(0.368f, 0, 0.671f, 1);

        door.GetComponent<BoxCollider2D>().enabled = !powered;
        door.GetComponent<SpriteRenderer>().color = powered? openedColor : closedColor;
    }

    private void SetWirePowered(GameObject wire, bool powered)
    {
        Color unpoweredColor = new Color(0.32f, 0.35f, 0);
        Color poweredColor = new Color(0.9f, 1, 0);
        wire.GetComponent<SpriteRenderer>().color = powered? poweredColor : unpoweredColor;
    }
}
