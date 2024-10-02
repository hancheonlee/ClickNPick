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

    public Animator star1anim;
    public Animator star2anim;
    public Animator star3anim;

    public int star1Threshold;
    public int star2Threshold;
    public int star3Threshold;

    public int maxClicks = 5;
    public int currentClicks = 0;

    [SerializeField] private Color grayColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    private Color originalColor = Color.white;

    private bool star1Earned = false;
    private bool star2Earned = false;
    private bool star3Earned = false;

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
        if (currentClicks >= star1Threshold && !star1Earned)
        {
            star1.color = originalColor;
            star1anim.enabled = true;
            Star1Action();
            star1Earned = true;
        }
        else if (currentClicks < star1Threshold)
        {
            star1.color = grayColor;
            star1Earned = false;
        }

        if (currentClicks >= star2Threshold && !star2Earned)
        {
            star2.color = originalColor;
            star2anim.enabled = true;
            Star2Action();
            star2Earned = true;
        }
        else if (currentClicks < star2Threshold)
        {
            star2.color = grayColor;
            star2Earned = false;
        }

        if (currentClicks >= star3Threshold && !star3Earned)
        {
            star3.color = originalColor;
            star3anim.enabled = true;
            Star3Action();
            star3Earned = true;
        }
        else if (currentClicks < star3Threshold)
        {
            star3.color = grayColor;
            star3Earned = false;
        }
    }

    private void Star1Action()
    {
        Debug.Log("Star 1 earned! Perform action here.");
        AudioManager.Instance.PlaySFX("Star");
    }

    private void Star2Action()
    {
        Debug.Log("Star 2 earned! Perform action here.");
        AudioManager.Instance.PlaySFX("Star");
    }

    private void Star3Action()
    {
        Debug.Log("Star 3 earned! Perform action here.");
        AudioManager.Instance.PlaySFX("Win");
    }
}
