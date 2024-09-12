using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorIdle;
    [SerializeField] private Texture2D cursorGrab;
    [SerializeField] private Texture2D cursorObject;
    [SerializeField] private Texture2D cursorCharacter;
    [SerializeField] private Texture2D cursorWater;

    public float clickHoldTime = 0.2f;
    private float clickTime = 0f;

    public Vector2 cursorHotspot;

    public cursorState currentState;

    public CameraMovement cameraMovement;

    public enum cursorState
    {
        Idle, Grabbing, Object, Character, Water
    }

    void Start()
    {
        Cursor.SetCursor(cursorIdle, cursorHotspot, CursorMode.Auto);

        cameraMovement = FindAnyObjectByType<CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        StateControl();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("InteractableObject") || hit.collider.CompareTag("ElectricBox")
                || hit.collider.CompareTag("ElectricBoxDoor") || hit.collider.CompareTag("Lamp")
                || hit.collider.CompareTag("Key") || hit.collider.CompareTag("LEDTV") || hit.collider.CompareTag("Leaves")
                || hit.collider.CompareTag("Bench") || hit.collider.CompareTag("Bone") || hit.collider.CompareTag("WaterPuddle"))
            {
                currentState = cursorState.Object;
            }
            else if (hit.collider.CompareTag("InteractableCharacter"))
            {
                currentState = cursorState.Character;
            }
            else if (hit.collider.CompareTag("Sprinkler"))
            {
                currentState = cursorState.Water;
            }
        }
        else
        {
            if (clickTime > clickHoldTime)
            {
                currentState = cursorState.Grabbing;
            }
            else
            {
                currentState = cursorState.Idle;
            }
        }

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
    }

    void StateControl()
    {
        if (currentState == cursorState.Grabbing)
        {
            Cursor.SetCursor(cursorGrab, cursorHotspot, CursorMode.Auto);
        }
        else if (currentState == cursorState.Object)
        {
            Cursor.SetCursor(cursorObject, cursorHotspot, CursorMode.Auto);
        }
        else if (currentState == cursorState.Character)
        {
            Cursor.SetCursor(cursorCharacter, cursorHotspot, CursorMode.Auto);
        }
        else if (currentState == cursorState.Water)
        {
            Cursor.SetCursor(cursorWater, cursorHotspot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(cursorIdle, cursorHotspot, CursorMode.Auto);
        }
    }
}
