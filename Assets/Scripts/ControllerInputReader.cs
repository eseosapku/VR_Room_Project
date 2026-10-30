using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerInputReader : MonoBehaviour
{
    [SerializeField] private InputActionReference gripAction;
    [SerializeField] private InputActionReference triggerAction;

    public event Action<float> GripChanged;
    public event Action<float> TriggerChanged;

    private float lastGrip = -1f;
    private float lastTrigger = -1f;

    private void Update()
    {
        float grip = gripAction.action.ReadValue<float>();
        float trigger = triggerAction.action.ReadValue<float>();
        if (!Mathf.Approximately(grip, lastGrip))
        {
            lastGrip = grip;
            if (GripChanged != null)
            {
                GripChanged.Invoke(grip);
            }
        }
        if (!Mathf.Approximately(trigger, lastTrigger))
        {
            lastTrigger = trigger;
            if (TriggerChanged != null)
            {
                TriggerChanged.Invoke(trigger);
            }
        }
    }
}
