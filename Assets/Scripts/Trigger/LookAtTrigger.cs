#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class LookAtTrigger : MonoBehaviour
{
    [Range(0.1f, 20f)]
    public float radius = 5f;

    [Range(0f, 360f)]
    public float FOVDegrees = 90f;

    [SerializeField]
    private float Threshold = 0.75f;

    public GameObject Target;
    public GameObject LookingAt;

    public bool Triggered = false;

    private bool IsTriggered()
    {
        Threshold = Mathf.Cos(Mathf.Deg2Rad * FOVDegrees / 2f);

        Vector3 trigger = transform.position;
        Vector3 target = Target.transform.position;
        Vector3 looking = LookingAt.transform.position;

        Vector3 trigger_to_target = target - trigger;
        Vector3 trigger_to_lookat = looking - trigger;

        // 1. Radial test
        if (trigger_to_target.magnitude > radius)
            return false;

        // 2. LookAt test (Dot Product)
        float dotp = Vector3.Dot(trigger_to_target.normalized, trigger_to_lookat.normalized);

        return dotp >= Threshold;
    }

    private void Update()
    {
        // Ensure the trigger state updates during gameplay
        if (Target != null && LookingAt != null)
            Triggered = IsTriggered();
    }

    private void OnDrawGizmos()
    {
        if (Target == null || LookingAt == null) return;

        Triggered = IsTriggered();

        if (Triggered)
            Handles.color = Color.red;
        else
            Handles.color = Color.green;

        Handles.DrawWireDisc(transform.position, Vector3.up, radius);

        Vector3 trigger = transform.position;
        Vector3 target = Target.transform.position;
        Vector3 looking = LookingAt.transform.position;
        Vector3 trigger_to_target = target - trigger;
        Vector3 trigger_to_lookat = looking - trigger;

        // Arrow at trigger
        if (trigger_to_lookat.sqrMagnitude > 0.0001f)
        {
            Color arrowColor = Triggered ? Color.red : Color.blue;
            Drawing.DrawVector(trigger_to_lookat.normalized * 2f, trigger, 2f, 0.4f, arrowColor);
        }

        // Arrow at LookingAt
        Drawing.DrawVector(LookingAt.transform.forward * 2f, looking, 2f, 0.4f, Color.cyan);

        // Arrow at Target
        Drawing.DrawVector(Target.transform.forward * 2f, target, 2f, 0.4f, Color.yellow);

        // Vector from trigger to target
        Drawing.DrawVector(trigger_to_target, trigger, 2f, 0.4f, Color.darkMagenta);

        // Field of view boundaries
        if (trigger_to_lookat.sqrMagnitude > 0.0001f)
        {
            Quaternion rotLeft = Quaternion.AngleAxis(-FOVDegrees / 2f, Vector3.up);
            Quaternion rotRight = Quaternion.AngleAxis(FOVDegrees / 2f, Vector3.up);

            Vector3 direction = radius * trigger_to_lookat.normalized;
            Vector3 leftDir = rotLeft * direction;
            Vector3 rightDir = rotRight * direction;

            Color col = Triggered ? Color.red : Color.green;
            Drawing.DrawVector(leftDir, trigger, 2f, 0.4f, col);
            Drawing.DrawVector(rightDir, trigger, 2f, 0.4f, col);

            Handles.DrawWireArc(trigger, Vector3.up, leftDir, FOVDegrees, radius);
        }
    }
}
#endif