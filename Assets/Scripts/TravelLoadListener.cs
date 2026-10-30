using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelLoadListener : MonoBehaviour
{
    [SerializeField] private float delay = 1f;

    private bool travelling;

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
        if (travelling)
        {
            return;
        }
        travelling = true;
        StartCoroutine(Load(destination));
    }

    private IEnumerator Load(string destination)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(destination);
    }
}
