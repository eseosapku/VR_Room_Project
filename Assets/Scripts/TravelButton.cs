using UnityEngine;

public class TravelButton : MonoBehaviour
{
    [SerializeField] private string destination;

    public void Travel()
    {
        TravelEvents.Request(destination);
    }
}
