using System.Collections.Generic;
using UnityEngine;

public class SphereScript : MonoBehaviour
{
    [Range(0.1f, 20f)]
    public float InnerRadius = 1.0f;

    [Range(0.1f, 20f)]
    public float OuterRadius = 3.0f;

    [Range(3, 264)]
    public int Segments = 12;

    private Mesh discmesh;
    private readonly List<Vector3> verts = new List<Vector3>();
    private readonly List<int> tri_indices = new List<int>();
    private MeshFilter _mf;
    private MeshRenderer _mr;

#if UNITY_EDITOR
    private bool _rebuildQueued = false;
#endif

    private void Start()
    {
        GenerateMesh();
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (_rebuildQueued)
            return;

        _rebuildQueued = true;
        UnityEditor.EditorApplication.delayCall += () =>
        {
            _rebuildQueued = false;
            if (this != null)
                GenerateMesh();
        };
#endif
    }

    private void GenerateMesh()
    {
        if (discmesh == null)
            discmesh = new Mesh();
        else
            discmesh.Clear();

        verts.Clear();
        tri_indices.Clear();

        //verts.Add(Vector3.zero); // Center point

        List<Vector2> uvs = new List<Vector2>();

        // Compute our vertices on the edge of the disc
        float delta_angle = 360f / Segments;
        Vector3 point = Vector3.zero;
        for (int i = 0; i < Segments; i++)
        {
            point.x = Mathf.Cos(Mathf.Deg2Rad * (delta_angle * i));
            point.y = Mathf.Sin(Mathf.Deg2Rad * (delta_angle * i));

            // Inner point
            verts.Add(point * InnerRadius);
            // Temporary variable for the uvcoord
            Vector2 uvcoord = new Vector2(point.x, point.y);
            // Compute the inner UV
            uvs.Add(0.5f * uvcoord * (InnerRadius / OuterRadius) + new Vector2(0.5f, 0.5f));

            // Outer point
            verts.Add(point * OuterRadius);
            uvs.Add(0.5f * uvcoord + new Vector2(0.5f, 0.5f));
        }

        discmesh.SetVertices(verts); // Assign vertices to the mesh
        discmesh.SetUVs(0, uvs);

        for (int i = 0; i < Segments - 1; i++)
        {
            tri_indices.Add(2 * i);
            tri_indices.Add(2 * i + 3);
            tri_indices.Add(2 * i + 1);

            tri_indices.Add(2 * i);
            tri_indices.Add(2 * i + 2);
            tri_indices.Add(2 * i + 3);
        }

        tri_indices.Add(verts.Count - 2);
        tri_indices.Add(1);
        tri_indices.Add(verts.Count - 1);

        tri_indices.Add(verts.Count - 2);
        tri_indices.Add(0);
        tri_indices.Add(1);

        discmesh.SetTriangles(tri_indices, 0);
        discmesh.RecalculateNormals();

        if (_mf == null)
            _mf = GetComponent<MeshFilter>();
        if (_mf == null)
            _mf = gameObject.AddComponent<MeshFilter>();

        if (_mr == null)
            _mr = GetComponent<MeshRenderer>();
        if (_mr == null)
            _mr = gameObject.AddComponent<MeshRenderer>();

        _mf.sharedMesh = discmesh;
    }
}