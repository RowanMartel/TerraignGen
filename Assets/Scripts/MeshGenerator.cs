using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    List<Vector3> vertices;
    List<int> triangles;

    [SerializeField] int planeWidth;
    [SerializeField] float heightMultiplier;
    [SerializeField] float inverseFrequency;
    [SerializeField] int iterations;

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
        transform.position = new Vector3(0, -(iterations * heightMultiplier) / 2, 0);
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

    public void Process()
    {
        ProcessUI();
        CreateShape();
        AdjustVertexHeight();
        UpdateMesh();
    }

    // UI methods and properties

    [SerializeField] GameObject properties;
    [SerializeField] GameObject collapseBtn;
    [SerializeField] GameObject expandBtn;
    [SerializeField] Slider meshSizeSlider;
    [SerializeField] Slider heightMultiplierSlider;
    [SerializeField] Slider inverseFrequencySlider;
    [SerializeField] Slider iterationsSlider;

    public void CollapseProperties()
    {
        properties.SetActive(false);
        collapseBtn.SetActive(false);
        expandBtn.SetActive(true);
    }
    public void ExpandProperties()
    {
        properties.SetActive(true);
        collapseBtn.SetActive(true);
        expandBtn.SetActive(false);
    }

    void ProcessUI()
    {
        planeWidth = (int)meshSizeSlider.value;
        heightMultiplier = heightMultiplierSlider.value;
        inverseFrequency = inverseFrequencySlider.value;
        iterations = (int)iterationsSlider.value;
    }
}