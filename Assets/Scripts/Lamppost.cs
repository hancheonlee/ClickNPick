using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamppost : MonoBehaviour
{
    private SpriteRenderer lampPostRenderer;
    public Sprite lampPostOpened;
    public Sprite lampPostClosed;
    public GameObject lights;
    public lampState currentState;

    public enum lampState
    {
        Opened, Closed
    }
    private void Start()
    {
        lampPostRenderer = GetComponent<SpriteRenderer>();
        currentState = lampState.Closed;
    }
    public void Update()
    {
        if (currentState == lampState.Opened)
        {
            lampPostRenderer.sprite = lampPostOpened;
            lights.SetActive(true);
        }
        else if (currentState == lampState.Closed)
        {
            lampPostRenderer.sprite= lampPostClosed;
            lights.SetActive(false);
        }
    }
}
