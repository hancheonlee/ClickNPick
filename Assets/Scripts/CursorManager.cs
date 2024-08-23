using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorIdle;
    [SerializeField] private Texture2D cursorGrab;
    [SerializeField] private Texture2D cursorObject;
    [SerializeField] private Texture2D cursorCharacter;

    public float clickHoldTime = 0.2f;
    private float clickTime = 0f;


    public cursorState currentState;

    public CameraMovement cameraMovement;

    public enum cursorState
    {
        Idle, Grabbing, Object, Character
    }

    void Start()
    {
        Cursor.SetCursor(cursorIdle, Vector2.zero, CursorMode.Auto);

        cameraMovement = FindAnyObjectByType<CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            clickTime = 0;
        }
        if (Input.GetMouseButton(0)) // Left-click
        {
            clickTime += Time.deltaTime; // Start timing
        }
        if (Input.GetMouseButtonUp(0)) // Left-click
        {
            clickTime = 0;
        }


        if (currentState == cursorState.Grabbing)
        {
            Cursor.SetCursor(cursorGrab, Vector2.zero, CursorMode.Auto);
        }
        else if (currentState == cursorState.Object)
        {
            Cursor.SetCursor(cursorObject, Vector2.zero, CursorMode.Auto);
        }
        else if (currentState == cursorState.Character)
        {
            Cursor.SetCursor(cursorCharacter, Vector2.zero, CursorMode.Auto);
        }
        else 
        {
            Cursor.SetCursor(cursorIdle, Vector2.zero, CursorMode.Auto);
        }

        if (clickTime > clickHoldTime)
        {
            currentState = cursorState.Grabbing;
        }
        else
        {
            currentState = cursorState.Idle;
        }

    }
}
