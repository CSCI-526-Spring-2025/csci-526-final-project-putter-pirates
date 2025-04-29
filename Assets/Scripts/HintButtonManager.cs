using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class HintButtonManager : MonoBehaviour
{
    public Button hintButton;
    private int shotCount = 0;
    private bool hintUnlocked = false;
    public TileInitializer tileInitializer;
    //public TextMeshProUGUI textMeshPro;


    private Color32 red = new Color32(255, 100, 100, 255);
    private Color32 white = new Color32(255, 255, 255, 255);
    //private Color32 black = new Color32(0, 0, 0, 255);

    void Start()
    {
        if (hintButton != null)
        {
            hintButton.interactable = false;
        }
        else
        {
            Debug.LogWarning("❗ Hint button not assigned in inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) UnlockHint(); // For testing unlock faster
    }

    public void OnBallShot()
    {
        shotCount++;

        if (!hintUnlocked && shotCount >= 5)
        {
            UnlockHint();
        }
    }

    void UnlockHint()
    {
        hintUnlocked = true;
        hintButton.interactable = true;
        StartCoroutine(PulseHintButton());
    }

    IEnumerator PulseHintButton()
    {
        hintButton.GetComponent<Image>().color = red;
        //textMeshPro.color = white;
        Transform buttonTransform = hintButton.transform;
        Vector3 originalScale = buttonTransform.localScale;
        float pulseSpeed = 4f;
        float pulseAmount = 0.2f;

        float timer = 0f;
        float duration = 5f;

        while (timer < duration)
        {
            float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            buttonTransform.localScale = originalScale * scale;

            timer += Time.deltaTime;
            yield return null;
        }

        buttonTransform.localScale = originalScale;
        hintButton.GetComponent<Image>().color = white;
        //textMeshPro.color = black;
    }

    public void OnHelpButtonClicked()
    {
        Debug.Log("Helpppppppp");
        tileInitializer.LockNextTileInOrder();
        GameAnalytics.instance.OnHelpButtonClicked(); // Track help usage
    }
}
