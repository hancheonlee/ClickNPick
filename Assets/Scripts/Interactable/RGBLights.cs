using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RGBLights : MonoBehaviour
{
    public Light2D light2D;       // Reference to the 2D Light component
    public float cycleDuration = 5f;  // Duration to complete one full color cycle

    private float time;

    void Start()
    {
        if (light2D == null)
        {
            light2D = GetComponent<Light2D>();
        }
    }

    void Update()
    {
        time += Time.deltaTime / cycleDuration;

        // Calculate the current color using Mathf.PingPong and HSV to RGB conversion
        float hue = Mathf.PingPong(time, 1);
        Color color = Color.HSVToRGB(hue, 1, 1);

        light2D.color = color;
    }
}
