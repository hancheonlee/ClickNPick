using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    [Header("Star Inactive Color")]
    [SerializeField] private Color grayColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    
    private Image starImage;
    private Animator animator;
    private Color originalColor = Color.white;
    private bool isEarned = false;

    private void Start()
    {
        starImage = GetComponent<Image>();
        animator = GetComponent<Animator>();
        starImage.color = grayColor;
    }
    public void UpdateStar(int currentClicks, int threshold)
    {
        if (currentClicks >= threshold && !isEarned)
        {
            EarnStar();
        }
        else if (currentClicks < threshold)
        {
            ResetStar();
        }
    }

    private void EarnStar()
    {
        starImage.color = originalColor;
        animator.enabled = true;
        isEarned = true;
        OnStarEarned();
    }

    private void ResetStar()
    {
        starImage.color = grayColor;
        animator.enabled = false; // Disable animation if needed
        isEarned = false;
    }

    protected virtual void OnStarEarned()
    {
        Debug.Log($"{name} earned! Perform action here.");
        AudioManager.Instance.PlaySFX("Star");
    }
}
