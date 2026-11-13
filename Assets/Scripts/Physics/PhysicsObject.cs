using System.Reflection.Metadata.Ecma335;
using UnityEngine;
using system.Collections.Generic;

public class PhysicsObject : MonoBehaviour
{
    [Header("Physics Object Properties")]
    [Range(0.001f, 10000f)]
    public float Mass = 1f; // In kilograms!!!
    public float GravityValue = 9.81f; // Default gravity (acceleration downwards
    public bool Gravity = true;


    [Header("Forces")]
    public List <IForce> Forcescv new List<IForce>();
    public Vector3 Force = Vector3.zero;
    public Vector3 ImpulseForce = Vector3.zero;
    public float ImpulseDuration = 0f;

    [Header("Internal stuff")]
    // Internally, we use inverse mass!!! Needs to be computed "somewhere"...
    [SerializeField]
    private float InverseMass;

    // Just for seeing the computed values
    [SerializeField]
    private Vector3 Position = Vector3.zero;

    [SerializeField]
    private Vector3 Velocity = Vector3.zero;

    [SerializeField]
    private Vector3 Acceleration = Vector3.zero;

    private float ImpulseTimer;

    private void UpdateInverseMass()
    {
        if (Mass > 0.00001f)
            InverseMass = 1.0f / Mass;
    }

    void Start()
    {
        UpdateInverseMass();

        Position = transform.position; // Move the internal position to
                                       // whatever the gameobject's position is

        ImpulseTimer = ImpulseDuration;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Physics calculation:
        // - Newton: F = ma --> a = 1/m * F
        // -        v = v0 + a*t
        // -        x = x0 + v*t  (+ 0.5*a*t*t)

        /*
        Vector3 ResultantForce = Force;
        if (ImpulseTimer > 0f)
        {
            ResultantForce += ImpulseForce;
            ImpulseTimer -= Time.fixedDeltaTime;
        }
        */
         Vector3 ResultantForce = Vector3.zero;
         foreach (var f in Forces)
         {
            
         }

        Acceleration = InverseMass * ResultantForce;

        if (Gravity)
            Acceleration.y -= GravityValue;

        Velocity = Velocity + Acceleration * Time.fixedDeltaTime;

        Position = Position + Velocity * Time.fixedDeltaTime;

        // Set the position of this gameobject
        transform.position = Position;
    }
}