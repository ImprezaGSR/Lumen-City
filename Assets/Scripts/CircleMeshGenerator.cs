using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class CircleMeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    public int xSize = 20;
    public int zSize = 20;
    public float amplitude = 2f;

    FastNoiseLite noise = new FastNoiseLite();
    public float[] noiseData;

    void Start()
    {
        noiseData = new float[xSize*zSize];

        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
        
        int index = 0;
        for (int y = 0; y < zSize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                noiseData[index++] = noise.GetNoise(x, y);
            }
        }
        // Do something with this data...

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
    }

    void CreateShape(){
        vertices = new Vector3[(xSize+1)*(zSize+1)];
        for (int i = 0, z = 0; z<= zSize; z++){
            for(int x = 0; x<= xSize; x++){
                float y = noiseData[i]*amplitude;
                vertices[i] = new Vector3(x,y,z);
                i++;
            }
        }
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++){
            for (int x = 0; x<xSize;x++){
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }
    }
    void UpdateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
    private void OnDrawGizmos()
    {
        if(vertices == null){
            return;
        }
        for(int i = 0; i < vertices.Length; i++){
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
    
}
