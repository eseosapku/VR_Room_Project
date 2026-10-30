using UnityEngine;

public class TooltipPlacement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float height = 0.2f;

    private Transform viewer;

    private void Start()
    {
        viewer = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.position = target.position + Vector3.up * height;
        Vector3 away = transform.position - viewer.position;
        away.y = 0f;
        if (away.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(away, Vector3.up);
        }
    }
}
