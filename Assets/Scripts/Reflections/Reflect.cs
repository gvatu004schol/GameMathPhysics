using UnityEngine;

public class Reflect : MonoBehaviour
{
    public GameObject Source;

    private void OnDrawGizmos()
    {
        if (Source == null) return;

        Vector3 origin = Source.transform.position;
        Vector3 ray    = Vector3.zero - origin;

        // Surface normal — up, since we reflect off the ground (y = 0)
        Vector3 normal = Vector3.up;

        // 1. Incoming ray: Source -> origin, tip lands on (0,0,0)
        Drawing.DrawVector(ray, origin, 0f, 0.3f, Color.blue);

        // 2. Normal at the origin
        Drawing.DrawVector(5f * normal, Vector3.zero, 0f, 0.3f, Color.lawnGreen);

        // 3. Reflection — same length as ray, mirrored across the normal
        Vector3 reflection = Vector3.Reflect(ray.normalized, normal);
        Drawing.DrawVector(reflection, Vector3.zero, ray.magnitude, 0.3f, new Color(0.6f, 0f, 1f));

        // 4. Projection of ray onto the normal
        Vector3 projected = Vector3.Dot(ray, normal) * normal;
        Drawing.DrawVector(projected, Vector3.zero, 0f, 0.3f, Color.white);

        // 5. ray drawn from origin (tip-to-tail reference)
        Drawing.DrawVector(ray, Vector3.zero, 0f, 0.3f, new Color(0.6f, 0f, 0f));

        // 6. -projected starting at the tip of ray
        Drawing.DrawVector(-projected, ray, 0f, 0.3f, Color.white);

        // 7. -projected starting at ray - projected (tip-to-tail)
        Drawing.DrawVector(-projected, ray - projected, 0f, 0.3f, Color.white);
    }
}