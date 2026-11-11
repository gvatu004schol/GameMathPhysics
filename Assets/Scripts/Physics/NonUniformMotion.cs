using UnityEngine;
using UnityEngine.InputSystem;

public class NonUniformMotion : MonoBehaviour
{
    public GameObject MovingObject;

    public Vector3 Pos0;
    public Vector3 Vel0;
    public Vector3 Accel; 

    private Vector3 vel; 

    void Start()
    {
        vel = Vel0;
        
        if (MovingObject != null)
        {
            MovingObject.transform.position = Pos0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(Pos0, 0.25f);

        Drawing.DrawVector(Vel0, Pos0, color: Color.cyan);

        Gizmos.color = Color.lightCoral;
        float delta_t = 0.01f; 

        Vector3 pos = Pos0;  
        Vector3 velSim = Vel0;  
        
        for (int i = 1; i <= 100; i++) 
        {
            pos = pos + velSim * delta_t;
            velSim = velSim + Accel * delta_t; 

            Gizmos.DrawSphere(pos, 0.10f);
        }
    }

    void Update()
    {
        Vector3 pos = MovingObject.transform.position;
        
        pos = pos + vel * Time.deltaTime;
        vel = vel + Accel * Time.deltaTime;

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            vel += Vector3.up * 5f;
        }
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            vel += Vector3.left * 5f;
        }
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            vel += Vector3.right * 5f;
        }
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            vel += Vector3.down * 5f;
        }
        
        MovingObject.transform.position = pos;
    }
}