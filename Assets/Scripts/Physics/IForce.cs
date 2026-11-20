using UnityEngine;

public interface IForce
{

// each force needs to add itself onto physcs object
//calculate itself, and the Physics object
// is responsible for adding it to the resultant forse

Vector3 CalculateForce(PhysicsObject body);    

}
