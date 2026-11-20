using UnityEngine;

public class ImpulseForses : MonoBehaviour, IForce

{

    public Vector3 Impulse;
    public float Duration = 1.0f;
    
    
    // each force needs to add itself onto physcs object
    //calculate itself, and the Physics object
        // is responsible for adding it to the resultant forse

   

    private float _elapsed = 0f;           // ADD this field

    public Vector3 CalculateForce(PhysicsObject body)   // ADD the parameter
        {
    if (_elapsed >= Duration) return Vector3.zero;
    _elapsed += Time.fixedDeltaTime;
    return Impulse;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

    }
    // Update is called once per frame
    private void Update()
    {

    }

}
