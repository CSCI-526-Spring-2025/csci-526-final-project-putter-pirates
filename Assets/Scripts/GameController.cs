using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool isRotateState = true;
    GameObject ball;
    GameObject fakeBall;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ball = GameObject.Find("Ball");
        fakeBall = GameObject.Find("FakeBall"); 

        ball.SetActive(!isRotateState);
        fakeBall.SetActive(isRotateState);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isRotateState = !isRotateState;
            Debug.Log("isRotateState: " + isRotateState);

            ball.SetActive(!isRotateState);
            ball.transform.position = fakeBall.transform.position;
            ball.GetComponent<Ball>().ResetPosition();
            fakeBall.SetActive(isRotateState);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            ball.GetComponent<Ball>().ResetPosition();
        }
    }

    public void Sucess()
    {
        ball.GetComponent<Ball>().Freeze();
    }
}
