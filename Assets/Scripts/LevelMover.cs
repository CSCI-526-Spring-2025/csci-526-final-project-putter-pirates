using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Required for coroutines

public class LevelMover : MonoBehaviour
{
    public int sceneBuildIndex;
    private Tile parentTile; // Reference to the Tile

    private void Start()
    {
        // Find the parent Tile object
        parentTile = GetComponentInParent<Tile>();
    }

    private void Update()
    {
        // If attached to a Tile, sync rotation with it
        if (parentTile != null)
        {
            transform.rotation = parentTile.transform.rotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DelayedSceneChange());
        }
    }

    private IEnumerator DelayedSceneChange()
    {
        yield return new WaitForSeconds(10); // Wait for 10 seconds
        SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
    }
}
