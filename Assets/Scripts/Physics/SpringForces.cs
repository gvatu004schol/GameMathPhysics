using UnityEngine;

public class SpringForces : MonoBehaviour, IForce
{
    public Transform AnchorPoint;
    public float NormalLenght = 1.0f;
    public float SpringConstant = 1.0f;

    public Vector3 CalculateForce(PhysicsObject pobj)
    {
        Vector3 v = pobj.transform.position - AnchorPoint.position;
        float deviation = v.magnitude - NormalLenght;


        return -SpringConstant * v.normalized * deviation;

    }
    private void OnDrawGizmos()
    {
    if (AnchorPoint == null) return;

    Gizmos.color = Color.black;
    Gizmos.DrawLine(AnchorPoint.position, transform.position);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
