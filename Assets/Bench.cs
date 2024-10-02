using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bench : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite bench;
    public Sprite dirtyBench;
    private BoxCollider2D boxCollider;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = dirtyBench;
    }

    public void CleanBench()
    {
        spriteRenderer.sprite = bench;
        boxCollider.enabled = false;
    }
}
