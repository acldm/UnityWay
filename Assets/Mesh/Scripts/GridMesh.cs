using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class GridMesh : MonoBehaviour
{
  public int xSize;
  public int ySize;

  private Vector3[] vertices;
  private Mesh mesh;
  private void Awake()
  {
    StartCoroutine(Generate());
  }

  private IEnumerator Generate()
  {
    WaitForSeconds wait = new WaitForSeconds(0.1f);
    GetComponent<MeshFilter>().mesh = mesh = new Mesh();
    vertices = new Vector3[(xSize + 1) * (ySize + 1)];
    Vector2[] uv = new Vector2[vertices.Length];
    for (int i = 0, ti = 0, y = 0; y <= ySize; y++) {
      for (int x = 0; x <= xSize; i++, ti += 6, x++) {
        vertices[i] = new Vector3(x,  y);
        uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
      }
    }
    mesh.vertices = vertices;
    mesh.uv = uv;
    
    int[] triangles = new int[xSize * ySize * 6];
    for (int i = 0, ti = 0, y = 0; y < ySize; i++, y++) {
      for (int x = 0; x < xSize; i++, ti += 6, x++) {
        triangles[ti] = i;
        triangles[ti + 1] = triangles[ti + 4] = i + xSize + 1;
        triangles[ti + 2] = triangles[ti + 3] = i + 1;
        triangles[ti + 5] = i + xSize + 2;
       
        yield return null;
      }
    }

    mesh.triangles = triangles;
    mesh.RecalculateNormals();
  }

  private void OnDrawGizmos() {
    Gizmos.color = Color.red;
    if (vertices == null) {
      return;
    }

    for (int i = 0; i < vertices.Length; i++) {
      if (vertices[i] == null) {
        continue;
      }

      Gizmos.DrawSphere(vertices[i], 0.1f);
    }


  }
}
