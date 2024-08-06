using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector3 Origin;
    private Vector3 Difference;

    private bool drag = false;


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

/*        // Touch Input
        if (Input.touchCount == 1) // Single touch
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Origin = Camera.main.ScreenToWorldPoint(touch.position);
            }
            if (touch.phase == TouchPhase.Moved)
            {
                Difference = (Camera.main.ScreenToWorldPoint(touch.position)) - Camera.main.transform.position;
                Camera.main.transform.position = Origin - Difference;
            }
        }*/

        if (drag)
        {
            Camera.main.transform.position = Origin - Difference;
        }

    }
}