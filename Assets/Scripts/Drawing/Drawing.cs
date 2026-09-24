using UnityEngine;
using UnityEditor;

public class Drawing
{
    public static void DrawVector(Vector3 pos, Vector3 vec, float thickness, float coneSize)
    {
        Vector3 end = pos + vec;

        Handles.DrawLine(pos, end, thickness);

        if (vec.sqrMagnitude < 0.000001f)
        {
            return;
        }

        float size = HandleUtility.GetHandleSize(end);

        Vector3 conePosition =
            end -
            vec.normalized *
            0.7071f *
            coneSize *
            size;

        Handles.ConeHandleCap(
            0,
            conePosition,
            Quaternion.LookRotation(vec),
            size * coneSize,
            EventType.Repaint
        );
    }
}