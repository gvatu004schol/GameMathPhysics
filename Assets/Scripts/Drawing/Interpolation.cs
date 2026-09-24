using UnityEngine;
using UnityEditor;

public class Interpolation : MonoBehaviour
{
    public GameObject GameObjectA;
    public GameObject GameObjectB;
    public GameObject GameObjectC;
    [Range(0f, 1f)]
    public float InterpolationT = 0.5f;
    private float CurrentTime = 0f;
    private bool GoingForward = true;
    [Range(1f, 20f)]
    public float InterpolationTime = 10f;

    private void OnDrawGizmos()
    {
        Handles.color = Color.green;

        Drawing.DrawVector(
            Vector3.zero,
            GameObjectA.transform.position,
            3f,
            0.4f
        );

        Handles.color = Color.darkOrange;

        Drawing.DrawVector(
            Vector3.zero,
            GameObjectB.transform.position,
            3f,
            0.4f
        );

        Handles.color = Color.purple;

        Drawing.DrawVector(
            GameObjectA.transform.position,
            GameObjectB.transform.position - GameObjectA.transform.position,
            3f,
            0.4f
        );
        //
        Vector3 interpolatedPosition =
    (1f - InterpolationT) * GameObjectA.transform.position
    + InterpolationT * GameObjectB.transform.position;

        GameObjectC.transform.position = interpolatedPosition;
        //
        Handles.color = Color.darkRed;

        Drawing.DrawVector(
            Vector3.zero,
            GameObjectC.transform.position,
            3f,
            0.4f
        );

        Vector3 part_of_a = (1f - InterpolationT) * GameObjectA.transform.position;

        Handles.color = Color.yellow;

        Drawing.DrawVector(
            Vector3.zero,
            part_of_a,
            3f,
            0.4f
        );

        Vector3 part_of_b = InterpolationT * GameObjectB.transform.position;

        Handles.color = Color.blue;

        Drawing.DrawVector(
            Vector3.zero,
            part_of_b,
            3f,
            0.4f
        );

        Vector3 vector_from_part_a_to_c = part_of_b;

        Handles.color = Color.blue;

        Drawing.DrawVector(
            part_of_a,
            vector_from_part_a_to_c,
            3f,
            0.4f
        );

        Vector3 vector_from_part_b_to_c = part_of_a;

        Handles.color = Color.yellow;

        Drawing.DrawVector(
            part_of_b,
            vector_from_part_b_to_c,
            3f,
            0.4f
        );
    }

   
    private void Update()
    {
        if (GoingForward)
        {
            CurrentTime += Time.deltaTime;
        }
        else
        {
            CurrentTime -= Time.deltaTime;
        }

        float t = CurrentTime / InterpolationTime;
        InterpolationT = t;
        if (t > 1f)
        {
            t = 1f;
            GoingForward = false;
        }

        if (t < 0f)
        {
            t = 0f;
            GoingForward = true;
        }

        Vector3 interpolatedPosition =
    (1f - t) * GameObjectA.transform.position
    + t * GameObjectB.transform.position;

        GameObjectC.transform.position = interpolatedPosition;
    }
}
