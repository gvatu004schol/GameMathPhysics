using UnityEngine;

public class CrossProduct : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
        Vector3 laserDirection = transform.right;
        RaycastHit hit;

        if (Physics.Raycast(origin, laserDirection, out hit))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, 0.02f);
            Gizmos.DrawLine(origin, hit.point);

            // Surface normal
            Drawing.DrawVector(5f * hit.normal, hit.point, 1f, 0.3f, Color.blue);

            // Cross product: perpendicular to both the ray and the normal
            Vector3 cross = Vector3.Cross( hit.normal, laserDirection);
            Drawing.DrawVector(5f* cross, hit.point, 1f, 0.3f, Color.red);

              Vector3 cross2 = Vector3.Cross(cross, hit.normal);
            Drawing.DrawVector(5f* cross2, hit.point, 1f, 0.3f, Color.green);


        }
    }
}