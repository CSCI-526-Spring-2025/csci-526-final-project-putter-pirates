using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameObject gc;
    public Rigidbody2D rb;
    public bool isDynamic = false;
    public bool freeze = true;  // don't move while in rotate mode

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gc = GameObject.Find("GameController");

        if (isDynamic)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            rb.simulated = false; // prevent the goal from falling before
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void flipFreeze()
    {
        freeze = !freeze;
        if (isDynamic)
        {
            if (freeze)
            {
                rb.simulated = false;
            }
            else
            {
                rb.simulated = true;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Debug.Log("Goal!");
            gc.GetComponent<GameController>().Sucess();
        }
    }
}
