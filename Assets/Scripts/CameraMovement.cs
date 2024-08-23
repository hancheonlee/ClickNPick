using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 Origin;
    private Vector3 Difference;

    private Vector3 targetPosition;

    public bool drag = false;

    public bool isFocusing;

    public float focusSpeed;

    public float minX, maxX, minY, maxY;

    private ZoomControl zoomControl;
    private void Start()
    {
        isFocusing = false;
        zoomControl = FindAnyObjectByType<ZoomControl>();
    }
    private void LateUpdate()
    {
        // Mouse Input
        if (Input.GetMouseButton(0))
        {
            Difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if (drag == false)
            {
                drag = true;
                Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;
        }

        if (drag)
        {
            Camera.main.transform.position = Origin - Difference;
            ClampCameraPosition();
        }

    }

    public void FocusMode(Vector3 target)
    {
        isFocusing = true;
        targetPosition = new Vector3(target.x, target.y, transform.position.z);
        zoomControl.isZooming = true;
    }

    private void Update()
    {
        if (isFocusing)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * focusSpeed);
        }
    }

    private void ClampCameraPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    private void OnDrawGizmos()
    {
        // Set the color of the Gizmos
        Gizmos.color = Color.yellow;

        // Draw a rectangle representing the camera boundaries
        Vector3 bottomLeft = new Vector3(minX, minY, 0);
        Vector3 topLeft = new Vector3(minX, maxY, 0);
        Vector3 topRight = new Vector3(maxX, maxY, 0);
        Vector3 bottomRight = new Vector3(maxX, minY, 0);

        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
    }

}