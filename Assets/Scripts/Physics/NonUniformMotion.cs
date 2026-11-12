using UnityEngine;
using UnityEngine.InputSystem;


public class CannonSimulation : MonoBehaviour
{
    [Header("Setup")]
    public GameObject Cannonball;
    public Vector3 StartPosition;
    public Vector3 Gravity;

    [Header("Cannon Settings")]
    [Range(0f, 90f)]
    public float ElevationAngle = 45f;
    public float MuzzleVelocity = 250f;
    public float TargetDistance = 2000f;

    [Header("Live Calculations")]
    public float ImpactDistance;
    public bool IsTargetHit;

    // Tracks the ball's speed during gameplay
    private Vector3 currentVelocity;

    private void OnDrawGizmos()
    {
        // Draw visual guides for the X and Y axes
        Drawing.DrawVector(Vector3.zero, 2000f * Vector3.right, 5f, 150f, Color.red);
        Drawing.DrawVector(Vector3.zero, 1000f * Vector3.up, 5f, 150f, Color.green);

        // Mark the spawn point
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(StartPosition, 100f);

        // Show the direction the cannon is pointing
        Drawing.DrawVector(StartPosition, currentVelocity.normalized * 300f, 5f, 150f, Color.darkCyan);

        // --- SIMULATE THE TRAJECTORY ---
        Gizmos.color = Color.lightCoral;
        float timeStep = 0.010f;
        float radianAngle = ElevationAngle * Mathf.Deg2Rad;

        Vector3 simPos = StartPosition;

        // Calculate initial speed split into X and Y directions
        Vector3 simVel = new Vector3(
            MuzzleVelocity * Mathf.Cos(radianAngle),
            MuzzleVelocity * Mathf.Sin(radianAngle),
            0f
        );

        // Step through time to draw the arc
        for (int step = 1; step <= 10000; step++)
        {
            simPos += simVel * timeStep;

            // Check if the ball hit the ground
            if (simPos.y < StartPosition.y && step > 5)
            {
                ImpactDistance = simPos.x - StartPosition.x;
                // Allow a 50-meter margin of error for the hit
                IsTargetHit = Mathf.Abs(ImpactDistance - TargetDistance) < 50f;

                // Draw a large yellow marker where it lands
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(simPos, 20f);
                break;
            }

            // Apply gravity to the velocity
            simVel += Gravity * timeStep;

            // Draw a big dot every 100 steps to show the path
            if (step % 100 == 0)
            {
                Gizmos.DrawSphere(simPos, 20f);
            }
        }
    }

    void Start()
    {
        // Setup the starting velocity vector for the actual gameplay
        float radianAngle = ElevationAngle * Mathf.Deg2Rad;

        currentVelocity.x = MuzzleVelocity * Mathf.Cos(radianAngle);
        currentVelocity.y = MuzzleVelocity * Mathf.Sin(radianAngle);
        currentVelocity.z = 0f;

        if (Cannonball != null)
        {
            Cannonball.transform.position = StartPosition;
        }
    }

    void Update()
    {
        if (Cannonball == null) return;

        // Move the actual ball frame by frame
        Vector3 framePos = Cannonball.transform.position;
        framePos += currentVelocity * Time.deltaTime;
        currentVelocity += Gravity * Time.deltaTime;

        // Allow the user to nudge the ball mid-flight using WASD
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.wasPressedThisFrame) currentVelocity += Vector3.up * 5f;
            if (Keyboard.current.sKey.wasPressedThisFrame) currentVelocity += Vector3.down * 5f;
            if (Keyboard.current.aKey.wasPressedThisFrame) currentVelocity += Vector3.left * 5f;
            if (Keyboard.current.dKey.wasPressedThisFrame) currentVelocity += Vector3.right * 5f;
        }

        // Apply the new position to the 3D object
        Cannonball.transform.position = framePos;
    }

  /*
 * --- WEEKLY ASSIGNMENT NOTES ---
 * Target: Sunila Factory (2000 meters away)
 * Weapon: 1600s Black Powder Cannon
 * Initial Muzzle Velocity (v0): 250 m/s
 * Gravity (g): 9.81 m/s^2
 * 
 * Using the Range Equation: R = (v0^2 * sin(2θ)) / g
 * Solving for θ: sin(2θ) = (2000 * 9.81) / (250^2) = 0.31392
 * 
 * Low Arc Angle (θ1): asin(0.31392) / 2 = 9.14 degrees
 * High Arc Angle (θ2): (180 - asin(0.31392)) / 2 = 80.86 degrees
 * 
 * Max Range (at 45 degrees): (250^2) / 9.81 = 6371 meters
 * Result: We can hit the target using either 9.14 or 80.86.
 */



}