using System;

public static class ClockEvents
{
    public static event Action<DateTime> Ticked;

    public static void Tick(DateTime time)
    {
        if (Ticked != null)
        {
            Ticked.Invoke(time);
        }
    }
}
