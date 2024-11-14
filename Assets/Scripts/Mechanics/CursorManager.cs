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

    private cursorState currentState;
    private cursorState lastState; // To track the last state

    public static bool enableCursor = true;

    public enum cursorState
    {
        Idle, Grabbing, Object, Character, Water
    }

    private void Start()
    {
        // Set the initial cursor to idle
        currentState = cursorState.Idle;
        UpdateCursor();
    }

    private void Update()
    {
        if (enableCursor)
        {
            StateControl();

            if (Input.GetMouseButtonDown(0)) // Left-click
            {
                clickTime = 0;
            }
            if (Input.GetMouseButton(0)) // While holding left-click
            {
                clickTime += Time.deltaTime;
            }
            if (Input.GetMouseButtonUp(0)) // Release left-click
            {
                clickTime = 0;
            }
        }
    }

    private void StateControl()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            // Check collider tags to update cursor state accordingly
            if (hit.collider.CompareTag("InteractableObject") || hit.collider.CompareTag("ElectricBox")
                || hit.collider.CompareTag("ElectricBoxDoor") || hit.collider.CompareTag("Lamp")
                || hit.collider.CompareTag("Key") || hit.collider.CompareTag("LEDTV") || hit.collider.CompareTag("Leaves")
                || hit.collider.CompareTag("Bench") || hit.collider.CompareTag("Bone") || hit.collider.CompareTag("WaterPuddle")
                || hit.collider.CompareTag("TrashCan"))
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
            currentState = clickTime > clickHoldTime ? cursorState.Grabbing : cursorState.Idle;
        }

        UpdateCursor(); // Call to update the cursor image if the state has changed
    }

    private void UpdateCursor()
    {
        // Only update the cursor if the state has changed
        if (currentState != lastState)
        {
            lastState = currentState; // Update last state to the current state
            switch (currentState)
            {
                case cursorState.Grabbing:
                    Cursor.SetCursor(cursorGrab, cursorHotspot, CursorMode.Auto);
                    break;
                case cursorState.Object:
                    Cursor.SetCursor(cursorObject, cursorHotspot, CursorMode.Auto);
                    break;
                case cursorState.Character:
                    Cursor.SetCursor(cursorCharacter, cursorHotspot, CursorMode.Auto);
                    break;
                case cursorState.Water:
                    Cursor.SetCursor(cursorWater, cursorHotspot, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(cursorIdle, cursorHotspot, CursorMode.Auto);
                    break;
            }
        }
    }
}
