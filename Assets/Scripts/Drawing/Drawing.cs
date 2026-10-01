using UnityEngine;

public static class Drawing
{
    public static void DrawVector(Vector3 vector, Vector3 origin, float length = 1f, float arrowHeadLength = 0.25f, Color color = default)
    {
        if (color == default) color = Color.white;
        Gizmos.color = color;

        Vector3 endPoint = origin + (vector.normalized * length);
        Gizmos.DrawLine(origin, endPoint);

        Vector3 right = Quaternion.LookRotation(vector) * Quaternion.Euler(0, 180 + 20, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(vector) * Quaternion.Euler(0, 180 - 20, 0) * new Vector3(0, 0, 1);

        Gizmos.DrawLine(endPoint, endPoint + right * arrowHeadLength);
        Gizmos.DrawLine(endPoint, endPoint + left * arrowHeadLength);
    }
}