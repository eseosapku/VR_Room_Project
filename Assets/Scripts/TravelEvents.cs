using System;

public static class TravelEvents
{
    public static event Action<string> Requested;

    public static void Request(string destination)
    {
        if (Requested != null)
        {
            Requested.Invoke(destination);
        }
    }
}
