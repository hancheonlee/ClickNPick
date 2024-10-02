using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightMover : MonoBehaviour
{
    public float slideDistance = 2f;  // How far the object will slide
    public float slideSpeed = 2f;     // How fast the object will slide
    public float slideDuration = 1f;  // How long the object will slide before reversing

    private Vector3 startPosition;
    private bool slidingLeft = true;
    private float timeElapsed = 0f;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // Calculate the sliding position using Mathf.PingPong for smooth back and forth movement
        float offset = Mathf.PingPong(timeElapsed * slideSpeed, slideDistance);

        if (slidingLeft)
        {
            transform.position = new Vector3(startPosition.x - offset, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(startPosition.x + offset, transform.position.y, transform.position.z);
        }

        // Reverse direction after the slide duration is over
        if (timeElapsed >= slideDuration)
        {
            slidingLeft = !slidingLeft;
            timeElapsed = 0f;
        }
    }
}
