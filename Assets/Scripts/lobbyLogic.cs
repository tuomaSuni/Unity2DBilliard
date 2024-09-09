using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class lobbyLogic : MonoBehaviour
{
    [SerializeField] private Image sr;
    [SerializeField] private TMP_Text waitingText;

    private float pulseSpeed = 2.0f;
    private float minAlpha = 45f / 255f;
    private float maxAlpha = 255f / 255f;

    private float currentAlpha;

    
    private float dotInterval = 0.5f;
    private string baseText = "Waiting for a player to join";
    private int dotCount = 0;
    private float timer;

    void Update()
    {
        ImageAnimation();
        TextAnimation();
    }

    private void ImageAnimation()
    {
        // Time-based sine wave oscillation for smooth pulsing effect
        float time = Time.time * pulseSpeed;
        
        // Use sine wave to oscillate between -1 and 1, then scale it to 0 to 1
        float sineWave = (Mathf.Sin(time) + 1f) / 2f; 

        // Remap sine wave (0-1) to alpha values (minAlpha to maxAlpha)
        currentAlpha = Mathf.Lerp(minAlpha, maxAlpha, sineWave);

        // Set new alpha value to the sprite color
        Color spriteColor = sr.color;
        spriteColor.a = currentAlpha;
        sr.color = spriteColor;
    }

    private void TextAnimation()
    {
        // Increment the timer based on time passed
        timer += Time.deltaTime;

        // Update the text at regular intervals (dotInterval)
        if (timer >= dotInterval)
        {
            // Reset the timer
            timer = 0f;

            // Update the number of dots (cycling between 0 and 3)
            dotCount = (dotCount + 1) % 4;

            // Create the new text with the current number of dots
            string dots = new string('.', dotCount);
            waitingText.text = baseText + dots;
        }
    }
}
