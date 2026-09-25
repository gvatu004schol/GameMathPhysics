using UnityEngine;

public class LookAtTrigger : MonoBehaviour
{
    [Header("Settings")]
    [Range(0.1f, 20f)]
    public float Radius = 5f;

    [Range(0f, 360f)]
    public float FOVDegrees = 90f; 

    [Header("References")]
    public GameObject Target;       // Drag your Player here
    public GameObject LookingAt;    // Drag your LookTarget here

    [Header("Debug State")]
    public bool Triggered = false;

    private bool CheckTrigger()
    {
        if (Target == null || LookingAt == null) return false;

        Vector3 triggerPos = transform.position;
        Vector3 targetPos = Target.transform.position;
        Vector3 lookingPos = LookingAt.transform.position;

        // 1. Distance Check (Radius)
        Vector3 trigger_to_target = targetPos - triggerPos;
        if (trigger_to_target.magnitude > Radius) return false;

        // 2. Angle Check (Dot Product)
        Vector3 trigger_to_lookat = (lookingPos - triggerPos).normalized;
        Vector3 targetDir = trigger_to_target.normalized;

        float dotProduct = Vector3.Dot(trigger_to_lookat, targetDir);
        float threshold = Mathf.Cos(Mathf.Deg2Rad * (FOVDegrees / 2f));

        return dotProduct > threshold;
    }

    private void Update()
    {
        Triggered = CheckTrigger();
        
        // You can add gameplay logic here!
        // if (Triggered) { Debug.Log("Player is in the wedge!"); }
    }

    private void OnDrawGizmos()
    {
        if (Target == null || LookingAt == null) return;

        // Update state in the editor so you can see it turn red while testing
        Triggered = CheckTrigger();

        Vector3 triggerPos = transform.position;
        Vector3 targetPos = Target.transform.position;
        Vector3 lookingPos = LookingAt.transform.position;

        Vector3 trigger_to_target = targetPos - triggerPos;
        Vector3 trigger_to_lookat = lookingPos - triggerPos;

        // --- 1. CALCULATE THE WEDGE CORNERS ---
        Quaternion leftRot = Quaternion.AngleAxis(-FOVDegrees / 2f, Vector3.up);
        Quaternion rightRot = Quaternion.AngleAxis(FOVDegrees / 2f, Vector3.up);

        Vector3 leftDir = leftRot * trigger_to_lookat.normalized;
        Vector3 rightDir = rightRot * trigger_to_lookat.normalized;

        Vector3 leftCorner = triggerPos + (leftDir * Radius);
        Vector3 rightCorner = triggerPos + (rightDir * Radius);
        
        // --- 2. DRAW THE WHITE WIREFRAME ---
        // (Turns red if triggered, just like your images)
        Gizmos.color = Triggered ? Color.red : Color.white;
        
        Gizmos.DrawLine(triggerPos, leftCorner);   // Left straight edge
        Gizmos.DrawLine(triggerPos, rightCorner);  // Right straight edge
        Gizmos.DrawLine(leftCorner, rightCorner);  // Front flat edge

        // Draw the curved front edge (using small line segments)
        int segments = 20;
        Vector3 previousPoint = leftCorner;
        for (int i = 1; i <= segments; i++)
        {
            float angle = Mathf.Lerp(-FOVDegrees / 2f, FOVDegrees / 2f, (float)i / segments);
            Quaternion stepRot = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 nextPoint = triggerPos + (stepRot * trigger_to_lookat.normalized * Radius);
            
            Gizmos.DrawLine(previousPoint, nextPoint);
            previousPoint = nextPoint;
        }

        // --- 3. DRAW THE MAGENTA LINES ---
        // (Connecting the Target to the outer corners of the wedge)
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(targetPos, leftCorner);
        Gizmos.DrawLine(targetPos, rightCorner);

        // --- 4. DRAW THE RED CENTER LINE ---
        Gizmos.color = Color.red;
        Gizmos.DrawLine(triggerPos, triggerPos + (trigger_to_lookat.normalized * Radius));

        // --- 5. DRAW DOTS FOR CLARITY ---
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(triggerPos, 0.15f); // Origin dot
        
        Gizmos.color = Triggered ? Color.red : Color.green;
        Gizmos.DrawSphere(targetPos, 0.2f); // Target dot
    }
}