using UnityEngine;

public class UniformMotion : MonoBehaviour
{
    public GameObject MovingObject;

    public Vector3 Pos0; // x0
    public Vector3 Vel;  // v
    public float Time = 0f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, 10f * Vector3.right);
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, 10f * Vector3.up);

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(Pos0, 0.25f);

        Drawing.DrawVector(Vel, Pos0, color: Color.blue);
        Drawing.DrawVector(Vel, Pos0, 3f, 0.4f, Color.blue);
        
        for (int i = 1; i <= 10; i++)
        {
            Vector3 position = Pos0 + i * Vel;
            Gizmos.DrawSphere(position, 0.25f);
        }
    }

    void Update()
    {
        Time += UnityEngine.Time.deltaTime;
        Vector3 position = Pos0 + Time * Vel;

        if (MovingObject != null)
        {
            MovingObject.transform.position = position;
        }
    }
}