using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite gateOpen;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void OpenGate()
    {
        spriteRenderer.sprite = gateOpen;
    }
}
