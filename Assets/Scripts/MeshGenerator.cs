using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    List<Vector3> vertices;
    List<int> triangles;

    [SerializeField] int planeWidth;
    [SerializeField] float heightMultiplier;
    [SerializeField] float inverseFrequency;
    [SerializeField] float iterations;

    void Start()
    {
        mesh = new();
        GetComponent<MeshFilter>().mesh = mesh;

        Process();
    }

    void CreateShape()
    {
        vertices = new();
        triangles = new();

        for (int i = 0; i < planeWidth + 1; i++)
        {
            for (int j = 0; j < planeWidth + 1; j++)
            {
                vertices.Add(new Vector3(i, 0, j));
            }
        }
        for (int i = 0; i < planeWidth; i++)
        {
            for (int j = 0; j < planeWidth; j++)
            {
                int currentVertex = j + (planeWidth + 1) * i;
                triangles.Add(currentVertex);
                triangles.Add(currentVertex + 1);
                triangles.Add(currentVertex + planeWidth + 1);

                triangles.Add(currentVertex + 1);
                triangles.Add(currentVertex + planeWidth + 2);
                triangles.Add(currentVertex + planeWidth + 1);
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    void AdjustVertexHeight()
    {
        for (int i = 1; i < iterations + 1; i++)
        {
            for (int j = 0; j < vertices.Count; j++)
            {
                vertices[j] = new Vector3(vertices[j].x, vertices[j].y + Mathf.PerlinNoise(vertices[j].x / inverseFrequency * i, vertices[j].z / inverseFrequency * i) * heightMultiplier, vertices[j].z);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Process();
    }

    void Process()
    {
        CreateShape();
        AdjustVertexHeight();
        UpdateMesh();
    }
}