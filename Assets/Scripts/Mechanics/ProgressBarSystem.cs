using UnityEngine;
using UnityEngine.UI;

public class ProgressBarSystem : MonoBehaviour
{
    [Header("Assign Components")]
    public Image green;
    public Star star1;
    public Star star2;
    public Star star3;

    [Header("Adjust Settings")]
    public int star1Threshold;
    public int star2Threshold;
    public int star3Threshold;

    [Space(10)]

    public int maxClicks = 5;


    private int currentClicks = 0;


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
            SaveLevel();
        }
    }

    private void UpdateProgressBar()
    {
        green.fillAmount = (float)currentClicks / maxClicks;
    }

    private void UpdateStars()
    {
        star1.UpdateStar(currentClicks, star1Threshold);
        star2.UpdateStar(currentClicks, star2Threshold);
        star3.UpdateStar(currentClicks, star3Threshold);
    }
    private void Star3Action()
    {
        Debug.Log("Star 3 earned! Perform action here.");
        AudioManager.Instance.PlaySFX("Win");
    }

    private void SaveLevel()
    {
        PlayerPrefs.SetInt("CurrentClicks", currentClicks);
        PlayerPrefs.Save();
    }

    private void LoadLevel()
    {
        currentClicks = PlayerPrefs.GetInt("CurrentClicks");
        UpdateProgressBar();
        UpdateStars();
    }
}
