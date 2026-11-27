using UnityEngine;

public class FluidDragForce : MonoBehaviour, IForce
{
public float WaterLevel = 0f;
public float DragCoefficient = 2000f;

public Vector3 CalculateForce(PhysicsObject pebj)
{
float height = pebj.transform.lossyScale.y;

// Bottom of the object
float bottom = pebj.transform.position.y - height / 2f;

// Submerged fraction
float submerged = Mathf.Clamp01(
(WaterLevel - bottom) / height
);

Vector3 vel = pebj.GetVelocity();

// Water resistance
return -vel * DragCoefficient * submerged;
}
}
