using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    public GameObject transitionCanvas;

    private float timer;
    public float timeMultiplier = 1.0f;

    private int hours;
    private int minutes;

    public bool skippingTime;

    private int newHours;
    private int newMinutes;

    public Button transitionButton;

    private void Start()
    {
        timer = 0;
    }
    private void Update()
    {
        timer += Time.deltaTime * timeMultiplier;

        newHours = (int)(timer / 3600) % 24;
        newMinutes = (int)((timer % 3600) / 60);

        if (newMinutes % 5 == 0 && (newMinutes != minutes || newHours != hours))
        {
            hours = newHours;
            minutes = newMinutes;

            timerText.text = string.Format("{0:00}:{1:00}", hours, minutes);
        }

        if (hours == 23 && minutes == 55 && !skippingTime)
        {
            skippingTime = true;
            StartCoroutine(TransitionNightToDay());

        }

        // Show button after 8 PM
        if (hours >= 20 && !skippingTime)
        {
            transitionButton.gameObject.SetActive(true);
        }
        else
        {
            transitionButton.gameObject.SetActive(false);
        }
    }

    public void TransitionButton()
    {
        skippingTime = true;
        StartCoroutine(TransitionNightToDay());
    }

    private IEnumerator TransitionNightToDay()
    {
        Debug.Log("Transition");
        transitionCanvas.SetActive(true);
        yield return new WaitForSeconds(4);
        hours = 8;
        minutes = 0;
        newHours = 8; 
        newMinutes = 0;
        timer = hours * 3600f + minutes * 60f;
        transitionCanvas.SetActive(false);
        skippingTime = false;
    }
}
