using UnityEngine;
using UnityEditor;

public class GameMathDrawing : MonoBehaviour
{
    public float ScreenWidth = 1920;
    public float ScreenHeight = 1080;
    public float ScreenPositionX = 0;
    public float ScreenPositionY = 0;
    public float PopupWidthPercent = 50;
    public float PopupHeightPercent = 50;
    public float HealthbarWidthPercent = 20;
    public float HealthbarHeightPercent = 20;
    public float HealthbarOffsetRight = 50;
    public float HealthbarOffsetTop = 50;
    public float thickness = 2;

    private void OnDrawGizmos()
    {
        Handles.color = Color.white;
        DrawRectangle(ScreenPositionX, ScreenPositionY, ScreenWidth, ScreenHeight, thickness);
        PopupPercent();
        HealthbarPercent();
        DrawVector(Vector3.zero, transform.position, thickness);
    }

    void DrawRectangle(float x, float y, float width, float height, float thickness)
    {
        Handles.DrawLine(new Vector3(x, y, 0), new Vector3(x + width, y, 0), thickness);
        Handles.DrawLine(new Vector3(x, y, 0), new Vector3(x, y + height, 0), thickness);
        Handles.DrawLine(new Vector3(x + width, y, 0), new Vector3(x + width, y + height, 0), thickness);
        Handles.DrawLine(new Vector3(x, y + height, 0), new Vector3(x + width, y + height, 0), thickness);
    }

    void PopupPercent()
    {
        float PopupWidth = ScreenWidth * (PopupWidthPercent / 100);
        float PopupHeight = ScreenHeight * (PopupHeightPercent / 100);
        float PopupX = ScreenPositionX + (ScreenWidth - PopupWidth) / 2;
        float PopupY = ScreenPositionY + (ScreenHeight - PopupHeight) / 2;
        DrawRectangle(PopupX, PopupY, PopupWidth, PopupHeight, thickness);
    }

    void HealthbarPercent()
    {
        float HealthbarWidth = ScreenWidth * (HealthbarWidthPercent / 100);
        float HealthbarHeight = ScreenHeight * (HealthbarHeightPercent / 100);
        float HealthbarX = (ScreenPositionX + ScreenWidth) - (HealthbarWidth + HealthbarOffsetRight);
        float HealthbarY = (ScreenPositionY + ScreenHeight) - (HealthbarHeight + HealthbarOffsetTop);
        DrawRectangle(HealthbarX, HealthbarY, HealthbarWidth, HealthbarHeight, thickness);
    }

    void DrawVector(Vector3 pos, Vector3 vec, float thickness)
    {
        Vector3 v = vec;
        float size = HandleUtility.GetHandleSize(v);
        float conesize = 0.4f;
        Handles.color = Color.orange;
        Handles.DrawLine(pos, v, thickness);
        Handles.ConeHandleCap(0, v - v.normalized * (Mathf.Sqrt(2) / 2.0f) * conesize * size,
    Quaternion.LookRotation(v), size * conesize, EventType.Repaint);

    }
}
