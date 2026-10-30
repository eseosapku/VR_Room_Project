using UnityEngine;

public class TravelFadeListener : MonoBehaviour
{
    [SerializeField] private FadeCanvas fade;

    private void OnEnable()
    {
        TravelEvents.Requested += OnTravelRequested;
    }

    private void OnDisable()
    {
        TravelEvents.Requested -= OnTravelRequested;
    }

    private void OnTravelRequested(string destination)
    {
        fade.StartFadeIn();
    }
}
