using System.Collections.Generic;
using UnityEngine;

public class FlatDonut : MonoBehaviour
{
    [Range(0.1f, 20f)]
    public float InnerRadius = 1.0f;

    [Range(0.1f, 20f)]
    public float OuterRadius = 3.0f;

    [Range(3, 264)]
    public int Segments = 12;

    private void GenerateMesh()
    {
        Mesh donutmesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<int> trl_indices = new List<int>();

        float delta_angle = 360f / Segments;
        Vector3 point = Vector3.zero;

        for (int i = 0; i < Segments; i++)
        {
            float cos = Mathf.Cos(Mathf.Deg2Rad * delta_angle * i);
            float sin = Mathf.Sin(Mathf.Deg2Rad * delta_angle * i);

            point.x = InnerRadius * cos;
            point.y = InnerRadius * sin;
            point.z = 0f;
            verts.Add(point);

            point.x = OuterRadius * cos;
            point.y = OuterRadius * sin;
            point.z = 0f;
            verts.Add(point);
        }

        for (int i = 0; i <= Segments - 2; i++)
        {
            int innerCurrent = 2 * i;
            int outerCurrent = 2 * i + 1;
            int innerNext = 2 * (i + 1);
            int outerNext = 2 * (i + 1) + 1;

            trl_indices.Add(innerCurrent);
            trl_indices.Add(outerNext);
            trl_indices.Add(outerCurrent);

            trl_indices.Add(innerCurrent);
            trl_indices.Add(innerNext);
            trl_indices.Add(outerNext);
        }

        int lastInner = 2 * (Segments - 1);
        int lastOuter = 2 * (Segments - 1) + 1;

        trl_indices.Add(lastInner);
        trl_indices.Add(1);
        trl_indices.Add(lastOuter);

        trl_indices.Add(lastInner);
        trl_indices.Add(0);
        trl_indices.Add(1);

        donutmesh.SetVertices(verts);
        donutmesh.SetTriangles(trl_indices, 0);
        donutmesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = donutmesh;
    }

    private void OnDrawGizmos()
    {
        GenerateMesh();
    }
}