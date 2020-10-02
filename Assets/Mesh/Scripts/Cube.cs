﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
  public int xSize = 5;
  public int ySize = 5;
  public int zSize = 5;
  public float roundness = 2.0f;
  private Vector3[] vertices;
  private Vector3[] normals;
  private Mesh mesh;
  void Start()
  {
    CreateCube();
  }

  public void CreateCube()
  {
    GetComponent<MeshFilter>().mesh = mesh = new Mesh();
    CreateVertices();
    CreateTriangles();
  }

  private void CreateVertices()
  {
    WaitForSeconds wait = new WaitForSeconds(0.01f);

    int cornerVertices = 8;
    int edgeVertices = (xSize + ySize + zSize - 3) * 4;
    int faceVertices = (
      (xSize - 1) * (ySize - 1) +
      (xSize - 1) * (zSize - 1) +
      (zSize - 1) * (ySize - 1)
    ) * 2;

    vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
    int i = 0;
    int v = 0;
    normals = new Vector3[vertices.Length];

    for (int y = 0; y <= ySize; y++)
    {

      for (int x = 0; x <= xSize; x++)
      {
        SetVertex(v++, x, y, 0);
      }

      for (int z = 1; z <= zSize; z++)
      {
        SetVertex(v++, xSize, y, z);
      }

      for (int x = xSize - 1; x >= 0; x--)
      {
        SetVertex(v++, x, y, zSize);
      }

      for (int z = zSize - 1; z > 0; z--)
      {
        SetVertex(v++, 0, y, z);
      }
    }
    for (int z = 1; z < zSize; z++)
    {
      for (int x = 1; x < xSize; x++)

      {
        SetVertex(v++, x, ySize, z);
      }
    }

    for (int z = 1; z < zSize; z++)
    {
      for (int x = 1; x < xSize; x++)
      {
        SetVertex(v++, x, 0, z);
      }
    }
    mesh.vertices = vertices;
    mesh.normals = normals;
  }

  private void CreateTriangles()
  {
    // 一共多少个小正方形
    int[] trianglesZ = new int[(xSize * ySize) * 12];
    int[] trianglesX = new int[(ySize * zSize) * 12];
    int[] trianglesY = new int[(xSize * zSize) * 12];

    //上一层点的相距甚远位置
    int ring = (xSize + zSize) * 2;
    int v = 0;
    int tZ = 0, tX = 0, tY = 0;
    for (int y = 0; y < ySize; y++, v++) {
			for (int q = 0; q < xSize; q++, v++) {
				tZ = SetQuad(trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
			}
			for (int q = 0; q < zSize; q++, v++) {
				tX = SetQuad(trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
			}
			for (int q = 0; q < xSize; q++, v++) {
				tZ = SetQuad(trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
			}
			for (int q = 0; q < zSize - 1; q++, v++) {
				tX = SetQuad(trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
			}
			tX = SetQuad(trianglesX, tX, v, v - ring + 1, v + ring, v + 1);
		}

    tY = CreateTopFace(trianglesY, tY, ring);
    tY = CreateBottomFace(trianglesY, tY, ring);

    mesh.subMeshCount = 3;
    mesh.SetTriangles(trianglesZ, 0);
    mesh.SetTriangles(trianglesX, 1);
    mesh.SetTriangles(trianglesY, 2);
  }

  int CreateTopFace(int[] triangles, int ring, int i)
  {
    int v = ySize * ring;
    for (int x = 0; x < xSize - 1; x++, v++)
    {
      i = SetQuad(triangles, i, v, v + 1, v + ring - 1, v + ring);
    }

    i = SetQuad(triangles, i, v, v + 1, v + ring - 1, v + 2);

    int vMin = ring * (ySize + 1) - 1;
    int vMid = vMin + 1;
    int vMax = v + 2;


    // i = SetQuad(triangles, i, vMin, vMid, vMin - 1, vMid + xSize - 1);
    // yield return wait;
    // for (int x = 1; x < xSize - 1; x++, vMid++)
    // {
    //   i = SetQuad(triangles, i, vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
    //   yield return wait;

    // }
    // i = SetQuad(triangles, i, vMid, vMax, vMid + xSize - 1, vMax + 1);
    for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++)
    {
      i = SetQuad(triangles, i, vMin, vMid, vMin - 1, vMid + xSize - 1);

      for (int x = 1; x < xSize - 1; x++, vMid++)
      {
        i = SetQuad(triangles, i, vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
      }

      i = SetQuad(triangles, i, vMid, vMax, vMid + xSize - 1, vMax + 1);
    }

    int vTop = vMin - 2;
    i = SetQuad(triangles, i, vMin, vMid, vTop + 1, vTop);
    for (int x = 1; x < xSize - 1; x++, vTop--, vMid++)
    {
      i = SetQuad(triangles, i, vMid, vMid + 1, vTop, vTop - 1);
    }
    i = SetQuad(triangles, i, vMid, vTop - 2, vTop, vTop - 1);

    return i;
  }

  int CreateBottomFace(int[] triangles, int ring, int i)
  {
    int v = 1;
    int vMid = vertices.Length - (xSize - 1) * (zSize - 1);
    i = SetQuad(triangles, i, ring - 1, vMid, 0, 1);

    for (int x = 1; x < xSize - 1; x++, v++, vMid++)
    {
      i = SetQuad(triangles, i, vMid, vMid + 1, v, v + 1);
    }

    i = SetQuad(triangles, i, vMid, v + 2, v, v + 1);

    int vMin = ring - 2;
    vMid -= xSize - 2;
    int vMax = v + 2;

    for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++)
    {
      i = SetQuad(triangles, i, vMin, vMid + xSize - 1, vMin + 1, vMid);

      for (int x = 1; x < xSize - 1; x++, vMid++)
      {
        i = SetQuad(triangles, i, vMid + xSize - 1, vMid + xSize, vMid, vMid + 1);
      }

      i = SetQuad(triangles, i, vMid + xSize - 1, vMax + 1, vMid, vMax);
    }

    int vTop = vMin - 1;
    i = SetQuad(triangles, i, vTop + 1, vTop, vTop + 2, vMid);
    for (int x = 1; x < xSize - 1; x++, vTop--, vMid++)
    {
      i = SetQuad(triangles, i, vTop, vTop - 1, vMid, vMid + 1);
    }
    i = SetQuad(triangles, i, vTop, vTop - 1, vMid, vTop - 2);

    return i;
  }

  private static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
  {
    triangles[i] = v00;
    triangles[i + 1] = triangles[i + 4] = v01;
    triangles[i + 2] = triangles[i + 3] = v10;
    triangles[i + 5] = v11;
    return i + 6;
  }
  private void OnDrawGizmos()
  {
    if (vertices == null)
    {
      return;
    }

    for (int i = 0; i < vertices.Length; i++)
    {
      Gizmos.color = Color.black;
      Gizmos.DrawSphere(vertices[i], 0.1f);
      Gizmos.color = Color.yellow;
      Gizmos.DrawRay(vertices[i], normals[i]);

    }
  }

  private void SetVertex(int i, int x, int y, int z)
  {
    Vector3 inner = vertices[i] = new Vector3(x, y, z);

    if (x < roundness)
    {
      inner.x = roundness;
    }
    else if (x > xSize - roundness)
    {
      inner.x = xSize - roundness;
    }

    if (y < roundness)
    {
      inner.y = roundness;
    }
    else if (y > ySize - roundness)
    {
      inner.y = ySize - roundness;
    }

    if (z < roundness)
    {
      inner.z = roundness;
    }
    else if (z > zSize - roundness)
    {
      inner.z = zSize - roundness;
    }
    normals[i] = (vertices[i] - inner).normalized;
    vertices[i] = inner + normals[i] * roundness;
  }
}
