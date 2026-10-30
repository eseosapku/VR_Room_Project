using UnityEngine;

public class HandAnimationListener : MonoBehaviour
{
    private static readonly int GripId = Animator.StringToHash("Grip");
    private static readonly int TriggerId = Animator.StringToHash("Trigger");

    [SerializeField] private ControllerInputReader reader;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        reader.GripChanged += OnGripChanged;
        reader.TriggerChanged += OnTriggerChanged;
    }

    private void OnDisable()
    {
        reader.GripChanged -= OnGripChanged;
        reader.TriggerChanged -= OnTriggerChanged;
    }

    private void OnGripChanged(float value)
    {
        animator.SetFloat(GripId, value);
    }

    private void OnTriggerChanged(float value)
    {
        animator.SetFloat(TriggerId, value);
    }
}
