using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HintButtonManager : MonoBehaviour
{
    public Button hintButton;
    private int shotCount = 0;
    private bool hintUnlocked = false;
    public TileInitializer tileInitializer;

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
        Transform buttonTransform = hintButton.transform;
        Vector3 originalScale = buttonTransform.localScale;
        float pulseSpeed = 4f;    // Speed of pulsing
        float pulseAmount = 0.1f; // How much to grow/shrink

        float timer = 0f;
        float duration = 5f; // Pulse for 3 seconds total

        while (timer < duration)
        {
            float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            buttonTransform.localScale = originalScale * scale;

            timer += Time.deltaTime;
            yield return null;
        }

        // Reset the button scale back to normal
        buttonTransform.localScale = originalScale;
    }

    public void OnHelpButtonClicked()
    {
        Debug.Log("Helpppppppp");
        tileInitializer.LockNextUnlockedTile();
    }

}
