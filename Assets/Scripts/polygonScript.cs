using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class polygonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreateSquarePolygon();
    }

    // Update is called once per frame
    void CreateSquarePolygon()
    {
        // Define the vertices of the square
        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(0, 0, 0);        // Bottom-left vertex
        vertices[1] = new Vector3(1, 0, 0);        // Bottom-right vertex
        vertices[2] = new Vector3(1, 1, 0);        // Top-right vertex
        vertices[3] = new Vector3(0, 1, 0);        // Top-left vertex

        // Define the triangles of the square
        int[] triangles = new int[6];
        triangles[0] = 0;   // Bottom-left
        triangles[1] = 1;   // Bottom-right
        triangles[2] = 2;   // Top-right

        triangles[3] = 0;   // Bottom-left
        triangles[4] = 2;   // Top-right
        triangles[5] = 3;   // Top-left

        // Create the mesh and set its properties
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Attach the mesh to a new MeshRenderer
        GameObject polygon = new GameObject("Polygon");
        MeshFilter meshFilter = polygon.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = polygon.AddComponent<MeshRenderer>();
        meshFilter.mesh = mesh;

        // You can set the material for the polygon here
        // Example:
        // meshRenderer.material = new Material(Shader.Find("Standard"));

        // Set the polygon's position (optional)
        polygon.transform.position = new Vector3(0, 0, 0);
    }
}
