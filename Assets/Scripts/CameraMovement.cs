using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 Origin;
    private Vector3 Difference;

    private Vector3 targetPosition;

    private bool drag = false;

    public bool isFocusing;

    public float focusSpeed;


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

}