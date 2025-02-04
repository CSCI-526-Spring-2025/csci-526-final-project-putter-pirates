using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameObject gc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gc = GameObject.Find("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Debug.Log("Goal!");
            gc.GetComponent<GameController>().Sucess();
        }
    }
}
