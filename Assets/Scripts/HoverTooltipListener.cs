using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HoverTooltipListener : MonoBehaviour
{
    [SerializeField] private XRBaseInteractable interactable;
    [SerializeField] private GameObject panel;

    private void OnEnable()
    {
        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
        interactable.selectEntered.AddListener(OnSelectEntered);
        interactable.selectExited.AddListener(OnSelectExited);
        Refresh();
    }

    private void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEntered);
        interactable.hoverExited.RemoveListener(OnHoverExited);
        interactable.selectEntered.RemoveListener(OnSelectEntered);
        interactable.selectExited.RemoveListener(OnSelectExited);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        Refresh();
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        Refresh();
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        Refresh();
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        Refresh();
    }

    private void Refresh()
    {
        bool byHand = false;
        foreach (IXRHoverInteractor interactor in interactable.interactorsHovering)
        {
            byHand |= !(interactor is XRSocketInteractor);
        }
        foreach (IXRSelectInteractor interactor in interactable.interactorsSelecting)
        {
            byHand |= !(interactor is XRSocketInteractor);
        }
        panel.SetActive(byHand);
    }
}
