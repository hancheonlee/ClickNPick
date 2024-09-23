using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public AudioClip clickSound;           // Sound effect for clicks
    public GameObject specialEffect;       // Particle effect
    public AudioClip specialEffectSound;   // Optional: Different sound for special effect
    public int maxClicks = 5;              // Max clicks before special effect or change

    private int clickCount = 0;            // Track how many times the object was clicked
    private Vector3 originalScale;
    private AudioSource audioSource;

    void Start()
    {
        originalScale = transform.localScale;
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        clickCount++;
        PlayClickSound();
        PlayVisualFeedback();

        if (clickCount >= maxClicks)  // If player reaches max clicks
        {
            TriggerSpecialEffect();
            clickCount = 0;  // Reset click count for the next stage
        }
    }

    void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    void PlayVisualFeedback()
    {
        // Make the object shrink
        transform.localScale = originalScale * 0.9f;
        Invoke("ResetScale", 0.1f);
    }

    void ResetScale()
    {
        transform.localScale = originalScale;
    }

    void TriggerSpecialEffect()
    {
        if (specialEffect != null)
        {
            Instantiate(specialEffect, transform.position, Quaternion.identity);
        }

        // Optionally play a special sound for reaching max clicks
        if (specialEffectSound != null)
        {
            audioSource.PlayOneShot(specialEffectSound);
        }
    }
}
