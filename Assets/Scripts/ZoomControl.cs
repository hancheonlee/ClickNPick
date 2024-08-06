using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomControl : MonoBehaviour
{
    public float zoomChange;
    public float smoothChange;
    public float minSize, maxSize;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        //Mouse Input
        if (Input.mouseScrollDelta.y > 0)
        {
            cam.orthographicSize -= zoomChange * Time.deltaTime * smoothChange;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            cam.orthographicSize += zoomChange * Time.deltaTime * smoothChange;
        }

        // Zoom with pinch gesture on mobile
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Calculate the previous and current distance between the touches
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;
            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            // Difference in the distances between each frame
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Adjust the orthographic size based on the change in distance between the touches
            cam.orthographicSize += deltaMagnitudeDiff * zoomChange/50 * Time.deltaTime * smoothChange;
        }

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minSize, maxSize);
    }
}
