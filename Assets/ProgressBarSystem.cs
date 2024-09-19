using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarSystem : MonoBehaviour
{
    public Image green;

    public Image star1;
    public Image star2;
    public Image star3;

    public int star1Threshold;  // Number of clicks required to enable star1
    public int star2Threshold;  // Number of clicks required to enable star2
    public int star3Threshold;  // Number of clicks required to enable star3

    public int maxClicks = 5;  
    public int currentClicks = 0;

    [SerializeField] private Color grayColor = new Color(0.5f, 0.5f, 0.5f, 1f);  // Gray color
    private Color originalColor = Color.white;  // Original color of the stars

    private void Start()
    {
        UpdateProgressBar();
        UpdateStars();
    }
    public void OnClick()
    {
        if (currentClicks < maxClicks)
        {
            currentClicks++;
            UpdateProgressBar();
            UpdateStars();
        }
    }

    private void UpdateProgressBar()
    {
        green.fillAmount = (float)currentClicks / maxClicks;
    }

    private void UpdateStars()
    {
        // Update star1
        if (currentClicks >= star1Threshold)
        {
            star1.color = originalColor;
        }
        else
        {
            star1.color = grayColor;
        }

        // Update star2
        if (currentClicks >= star2Threshold)
        {
            star2.color = originalColor;
        }
        else
        {
            star2.color = grayColor;
        }

        // Update star3
        if (currentClicks >= star3Threshold)
        {
            star3.color = originalColor;
        }
        else
        {
            star3.color = grayColor;
        }
    }
}
