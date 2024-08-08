using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private float timer;
    public float timeMultiplier = 1.0f;

    private int hours;
    private int minutes;

    private void Start()
    {
        timer = 0;
    }
    private void Update()
    {
        timer += Time.deltaTime * timeMultiplier;

        int newHours = (int)(timer / 3600) % 24;
        int newMinutes = (int)((timer % 3600) / 60);

        if (newMinutes % 5 == 0 && (newMinutes != minutes || newHours != hours))
        {
            hours = newHours;
            minutes = newMinutes;

            timerText.text = string.Format("{0:00}:{1:00}", hours, minutes);
        }
    }
}
