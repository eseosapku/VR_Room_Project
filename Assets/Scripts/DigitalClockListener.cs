using System;
using TMPro;
using UnityEngine;

public class DigitalClockListener : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private string accentColour = "#4FD1C5";

    private int lastSecond = -1;

    private void OnEnable()
    {
        ClockEvents.Ticked += OnTicked;
    }

    private void OnDisable()
    {
        ClockEvents.Ticked -= OnTicked;
    }

    private void OnTicked(DateTime time)
    {
        if (time.Second == lastSecond)
        {
            return;
        }
        lastSecond = time.Second;
        timeText.text = time.ToString("HH:mm") + "<size=55%><color=" + accentColour + ">:" + time.ToString("ss") + "</color></size>";
        dateText.text = time.ToString("dddd, d MMMM yyyy");
    }
}
