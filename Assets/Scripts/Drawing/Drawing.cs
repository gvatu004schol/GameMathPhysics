using UnityEngine;

public static class Drawing
{
    public static void DrawVector(Vector3 vector, Vector3 origin,
        float length = 1f, float arrowHeadLength = 0.25f, Color color = default)
    {
        if (color == default) color = Color.white;
        Gizmos.color = color;
        if (vector.sqrMagnitude < 1e-6f) return;

        Vector3 dir = vector.normalized;

        // length <= 0 means "use the vector's own magnitude"
        float drawLen = length > 0f ? length : vector.magnitude;

        Vector3 end = origin + dir * drawLen;
        Gizmos.DrawLine(origin, end);

        Quaternion rot = Quaternion.LookRotation(dir);
        Gizmos.DrawLine(end, end + rot * Quaternion.Euler(0, 160, 0) * Vector3.forward * arrowHeadLength);
        Gizmos.DrawLine(end, end + rot * Quaternion.Euler(0, 200, 0) * Vector3.forward * arrowHeadLength);
    }
}