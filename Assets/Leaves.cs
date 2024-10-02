using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaves : MonoBehaviour
{
    public float fadeDuration = 1.0f; // Time in seconds for the fade-out to complete

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private Color originalColor;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOutRoutine());
        boxCollider.enabled = false;
    }

    private IEnumerator FadeOutRoutine()
    {
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
    }
}
