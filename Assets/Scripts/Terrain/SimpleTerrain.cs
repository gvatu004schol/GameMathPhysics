using System.Collections.Generic;
using UnityEngine;

public class SimplePlane : MonoBehaviour
{
    [Range(1, 100)]
    public int Segments = 10;

    [Range(0.1f, 1000f)]
    public float Size = 10f;

    public float NoiseAmplitude = 10f;
    [Range(0.01f, 2.0f)]
    public float NoiseFrequency = 1.0f;
    [Range(0f, 20f)]
    public float NoiseOffsetX = 0.0f;
    [Range(0f, 20f)]
    public float NoiseOffsetY = 0.0f;

    public Mesh mymesh;

    private Mesh _mesh;
    private MeshFilter _mf;

#if UNITY_EDITOR
    private bool _rebuildQueued = false;
#endif

    private void Start()
    {
        Regenerate();
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (_rebuildQueued) return;
        _rebuildQueued = true;
        UnityEditor.EditorApplication.delayCall += () =>
        {
            _rebuildQueued = false;
            if (this != null) Regenerate();
        };
#endif
    }

    private void Regenerate()
    {
        _mesh = GenerateMesh();
        mymesh = _mesh;

        _mf = GetComponent<MeshFilter>();
        if (_mf == null)
            _mf = gameObject.AddComponent<MeshFilter>();

        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr == null)
            mr = gameObject.AddComponent<MeshRenderer>();

        _mf.sharedMesh = _mesh;
    }

    private Mesh GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "SimplePlane";

        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tri_indexes = new List<int>();

        float delta = Size / (float)Segments;

        Vector3 point = Vector3.zero;
        for (int i = 0; i <= Segments; i++)
        {
            for (int j = 0; j <= Segments; j++)
            {
                point.x = j * delta;
                point.z = i * delta;

                // === NOISE ===
                // Sample Perlin noise at (x, z) using the frequency and offsets,
                // and displace the vertex along the Y axis.
                float xCoord = (point.x + NoiseOffsetX) * NoiseFrequency;
                float yCoord = (point.z + NoiseOffsetY) * NoiseFrequency;

                point.y = Mathf.PerlinNoise(xCoord, yCoord) * NoiseAmplitude;

                verts.Add(point);
                uvs.Add(new Vector2(j / (float)Segments, i / (float)Segments));
            }
        }

        for (int i = 0; i < Segments; i++)
        {
            for (int j = 0; j < Segments; j++)
            {
                int lower_left  = i * (Segments + 1) + j;
                int lower_right = lower_left + 1;

                int upper_left  = (i + 1) * (Segments + 1) + j;
                int upper_right = upper_left + 1;

                tri_indexes.Add(lower_left);
                tri_indexes.Add(upper_right);
                tri_indexes.Add(lower_right);

                tri_indexes.Add(lower_left);
                tri_indexes.Add(upper_left);
                tri_indexes.Add(upper_right);
            }
        }

        mesh.SetVertices(verts);
        mesh.SetTriangles(tri_indexes, 0);
        mesh.SetUVs(0, uvs);
        mesh.RecalculateNormals();
        return mesh;
    }
}