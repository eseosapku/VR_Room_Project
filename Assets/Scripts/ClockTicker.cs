using System;
using UnityEngine;

public class ClockTicker : MonoBehaviour
{
    private void Update()
    {
        ClockEvents.Tick(DateTime.Now);
    }
}
