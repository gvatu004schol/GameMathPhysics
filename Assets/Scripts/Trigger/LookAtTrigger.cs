using UnityEngine;

public class LookAtTrigger : MonoBehaviour
{
    [Header("Settings")]
    [Range(0.1f, 20f)]
    public float Radius = 5f;

    [Range(0f, 360f)]
    public float FOVDegrees = 90f; // Horizontal angle (Left/Right)

    [Range(0f, 180f)]
    public float VerticalFOVDegrees = 45f; // Vertical angle (Up/Down)

    [Header("References")]
    public GameObject Target;      
    public GameObject LookingAt;   

    [Header("Debug State")]
    public bool Triggered = false;

    private bool CheckTrigger()
    {
        if (Target == null || LookingAt == null) return false;

        Vector3 triggerPos = transform.position;
        Vector3 targetPos = Target.transform.position;
        Vector3 lookingPos = LookingAt.transform.position;

        
        // TEST 1: RADIAL TRIGGER (Distance)
       
        Vector3 trigger_to_target = targetPos - triggerPos;
        if (trigger_to_target.magnitude > Radius) return false; // Too far away!

        
        // CALCULATE LOCAL TARGET POSITION
        
        // We convert the target's world position into the wedge's "local space". 
        // This means the center of the wedge is always (0,0,1).
        Quaternion forwardRotation = Quaternion.LookRotation(lookingPos - triggerPos, Vector3.up);
        Vector3 localTargetDir = Quaternion.Inverse(forwardRotation) * trigger_to_target.normalized;

        
        // TEST 2: LOOK-AT TRIGGER (Horizontal Angle)
        
        // Calculate the angle left or right from the center
        float horizontalAngle = Mathf.Atan2(localTargetDir.x, localTargetDir.z) * Mathf.Rad2Deg;
        if (Mathf.Abs(horizontalAngle) > FOVDegrees / 2f) return false;

        
        // TEST 3: HEIGHT TRIGGER (Vertical Angle)

        // Calculate the angle up or down from the center
        float verticalAngle = Mathf.Asin(localTargetDir.y) * Mathf.Rad2Deg;
        if (Mathf.Abs(verticalAngle) > VerticalFOVDegrees / 2f) return false;

        return true;
    }

    private void Update()
    {
        Triggered = CheckTrigger();
    }

    private void OnDrawGizmos()
    {
        if (Target == null || LookingAt == null) return;

        Triggered = CheckTrigger();

        Vector3 triggerPos = transform.position;
        Vector3 targetPos = Target.transform.position;
        Vector3 lookingPos = LookingAt.transform.position;

        Vector3 forwardDir = (lookingPos - triggerPos).normalized;
        Quaternion forwardRotation = Quaternion.LookRotation(forwardDir, Vector3.up);

        Color wedgeColor = Triggered ? Color.red : Color.white;
        Gizmos.color = wedgeColor;

        //  THE 3D CHEESE WEDGE 

        float hHalf = FOVDegrees / 2f;
        float vHalf = VerticalFOVDegrees / 2f;

        int hSteps = 10;
        int vSteps = 5;

        // 1. THE FRONT CURVED SURFACE (Grid)
        // Horizontal arcs
        for (int j = 0; j <= vSteps; j++)
        {
            float vAngle = Mathf.Lerp(-vHalf, vHalf, (float)j / vSteps);
            Vector3 prevPoint = Vector3.zero;
            for (int i = 0; i <= hSteps; i++)
            {
                float hAngle = Mathf.Lerp(-hHalf, hHalf, (float)i / hSteps);
                Vector3 currentPoint = triggerPos + (forwardRotation * Quaternion.Euler(vAngle, hAngle, 0) * Vector3.forward * Radius);
                if (i > 0) Gizmos.DrawLine(prevPoint, currentPoint);
                prevPoint = currentPoint;
            }
        }

        // Vertical arcs
        for (int i = 0; i <= hSteps; i++)
        {
            float hAngle = Mathf.Lerp(-hHalf, hHalf, (float)i / hSteps);
            Vector3 prevPoint = Vector3.zero;
            for (int j = 0; j <= vSteps; j++)
            {
                float vAngle = Mathf.Lerp(-vHalf, vHalf, (float)j / vSteps);
                Vector3 currentPoint = triggerPos + (forwardRotation * Quaternion.Euler(vAngle, hAngle, 0) * Vector3.forward * Radius);
                if (j > 0) Gizmos.DrawLine(prevPoint, currentPoint);
                prevPoint = currentPoint;
            }
        }

        // 2. THE STRAIGHT SPOKES (From origin to the 4 corners)
        Vector3 topLeft = triggerPos + (forwardRotation * Quaternion.Euler(-vHalf, -hHalf, 0) * Vector3.forward * Radius);
        Vector3 topRight = triggerPos + (forwardRotation * Quaternion.Euler(-vHalf, hHalf, 0) * Vector3.forward * Radius);
        Vector3 bottomLeft = triggerPos + (forwardRotation * Quaternion.Euler(vHalf, -hHalf, 0) * Vector3.forward * Radius);
        Vector3 bottomRight = triggerPos + (forwardRotation * Quaternion.Euler(vHalf, hHalf, 0) * Vector3.forward * Radius);

        Gizmos.DrawLine(triggerPos, topLeft);
        Gizmos.DrawLine(triggerPos, topRight);
        Gizmos.DrawLine(triggerPos, bottomLeft);
        Gizmos.DrawLine(triggerPos, bottomRight);

        // 3. THE MAGENTA LINES (Target to the 4 corners)
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(targetPos, topLeft);
        Gizmos.DrawLine(targetPos, topRight);
        Gizmos.DrawLine(targetPos, bottomLeft);
        Gizmos.DrawLine(targetPos, bottomRight);

        // 4. THE RED CENTER LINE
        Gizmos.color = Color.red;
        Gizmos.DrawLine(triggerPos, triggerPos + (forwardDir * Radius));

        // 5. DOTS
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(triggerPos, 0.15f);

        Gizmos.color = Triggered ? Color.red : Color.green;
        Gizmos.DrawSphere(targetPos, 0.2f);
    }
}