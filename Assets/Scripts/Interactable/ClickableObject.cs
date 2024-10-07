using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public GameObject specialEffect;       // Particle effect
    public int maxClicks = 5;              // Max clicks before special effect or change
    public float fadeDuration = 1.0f;      // Time needed to fade out
    public bool addProgress;

    public Sprite bench;
    public Sprite trashCanClean;

    private int clickCount = 0;            // Track how many times the object was clicked
    private Vector3 originalScale;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool actionCompleted = false;
    private ProgressBarSystem progressBarSystem;
    void Start()
    {
        originalScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        progressBarSystem = FindAnyObjectByType<ProgressBarSystem>();
    }

    void OnMouseDown()
    {
        if (CameraSystem.free)
        {
            if (!actionCompleted)
            {
                clickCount++;
                PlayClickSound();
                PlayVisualFeedback();

                if (clickCount >= maxClicks)  // If player reaches max clicks
                {
                    TriggerSpecialEffect();
                    if (addProgress)
                    {
                        progressBarSystem.OnClick();
                    }
                    clickCount = 0;  // Reset click count for the next stage
                }
            }
            else
            {
                TriggerSpecialEffect();
                PlayVisualFeedback();
            }
        }
    }

    void PlayClickSound()
    {
        AudioManager.Instance.PlaySFX("Click");
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
            AudioManager.Instance.PlaySFX("Special");
            GameObject VFX = Instantiate(specialEffect, transform.position, Quaternion.identity);
            Destroy(VFX, 2f);

            actionCompleted = true;
        }

        switch(gameObject.tag)
        {
            case "Leaves":
                FadeOut();
                break;
            case "Bench":
                ChangeSprite();
                break;
            case "TrashCan":
                ChangeSprite();
                break;
            case "Sprinkler":
                PlayAnimation();
                break;
            case "Car":
                PlaySFX();
                break;
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    public void ChangeSprite()
    {
        if (gameObject.CompareTag("Bench"))
        {
            spriteRenderer.sprite = bench;
        }
        else if (gameObject.CompareTag("TrashCan"))
        {
            spriteRenderer.sprite = trashCanClean;
        }

    }

    public void PlayAnimation()
    {
        Animator animator = gameObject.GetComponent<Animator>();
        if (gameObject.CompareTag("Sprinkler"))
        {
            animator.SetTrigger("Grow");
        }
    }

    public void PlaySFX()
    {
        actionCompleted = true;
        if (gameObject.CompareTag("Car"))
        {
            AudioManager.Instance.PlaySFX("Engine");
        }
    }

    private IEnumerator FadeOutRoutine()
    {
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        collider.enabled = false;
        float timer = 0f;
        Color currentColor = spriteRenderer.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alphaValue = Mathf.Lerp(originalColor.a, 0, timer / fadeDuration);
            spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, alphaValue);

            yield return null;
        }
        spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
        Destroy(gameObject, 2f);
    }
}
