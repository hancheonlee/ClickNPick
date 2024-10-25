using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableImage : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 originalScale;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    public void PlayVisualFeedback()
    {
        // Shrink the canvas object
        rectTransform.localScale = originalScale * 0.9f;
        Invoke("ResetScale", 0.1f);
        PlaySound();
    }

    void ResetScale()
    {
        // Reset to the original scale
        rectTransform.localScale = originalScale;
    }

    void PlaySound()
    {
        if (gameObject.tag == "Spider")
        {
            AudioManager.Instance.PlaySFX("Spider");
        }
    }
}
