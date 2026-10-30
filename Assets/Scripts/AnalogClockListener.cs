using System;
using UnityEngine;

public class AnalogClockListener : MonoBehaviour
{
    [SerializeField] private Transform hourHand;
    [SerializeField] private Transform minuteHand;
    [SerializeField] private Transform secondHand;
    [SerializeField] private Vector3 clockwiseAxis = Vector3.up;
    [SerializeField] private bool sweepSeconds = true;

    private Quaternion hourRest;
    private Quaternion minuteRest;
    private Quaternion secondRest;

    private void Awake()
    {
        hourRest = hourHand.localRotation;
        minuteRest = minuteHand.localRotation;
        secondRest = secondHand.localRotation;
    }

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
        float seconds = time.Second + (sweepSeconds ? time.Millisecond / 1000f : 0f);
        float minutes = time.Minute + seconds / 60f;
        float hours = time.Hour % 12 + minutes / 60f;
        secondHand.localRotation = secondRest * Quaternion.AngleAxis(seconds * 6f, clockwiseAxis);
        minuteHand.localRotation = minuteRest * Quaternion.AngleAxis(minutes * 6f, clockwiseAxis);
        hourHand.localRotation = hourRest * Quaternion.AngleAxis(hours * 30f, clockwiseAxis);
    }
}
