using UnityEngine;

public class BuoyancyForce : MonoBehaviour, IForce
{
public float WaterLevel = 0f;
public float WaterDensity = 1000f;

public Vector3 CalculateForce(PhysicsObject pebj)
{
Vector3 size = pebj.transform.lossyScale;

// Object height and volume
float height = size.y;
float volume = size.x * size.y * size.z;

// Bottom of the object
float bottom = pebj.transform.position.y - height / 2f;

// Submerged fraction
float submerged = Mathf.Clamp01(
(WaterLevel - bottom) / height
);

// Buoyancy force
float force = WaterDensity * volume * submerged * pebj.GravityValue;

return Vector3.up * force;
}
}
