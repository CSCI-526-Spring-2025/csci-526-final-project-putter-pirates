using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HintButtonManager : MonoBehaviour
{
    public Button hintButton;
    private int shotCount = 0;
    private bool hintUnlocked = false;

    void Start()
    {
        // Ensure the button starts locked
        if (hintButton != null)
        {
            hintButton.interactable = false;
        }
        else
        {
            Debug.LogWarning("❗ Hint button not assigned in inspector.");
        }
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
        StartCoroutine(FlashHintButton());
    }

    IEnumerator FlashHintButton()
    {
        Image btnImage = hintButton.GetComponent<Image>();
        Color originalColor = btnImage.color;
        Color flashColor = Color.yellow;

        for (int i = 0; i < 6; i++) // Flash 3 times
        {
            btnImage.color = flashColor;
            yield return new WaitForSeconds(0.2f);
            btnImage.color = originalColor;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
