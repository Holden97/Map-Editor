//Author：GuoYiBo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMesh : MonoBehaviour
{
    private int length = 3;
    private int width = 3;
    private int height = 3;

    private int totalVerticesCount;
    private Vector3[] vertices;
    private int curVertexIndex = 0;

    private Mesh mesh;

    private void Awake()
    {
        int cornerVerticesCount = 8;
        int edgeVerticesCount = ((length - 1) + (width - 1) + (height - 1)) * 4;
        int planesCount = ((height - 1) * (width - 1) + (height - 1) * (length - 1) + (length - 1) * (width - 1)) * 2;

        totalVerticesCount = cornerVerticesCount + edgeVerticesCount + planesCount;
        vertices = new Vector3[totalVerticesCount];

        var wait = new WaitForSeconds(0.1f);
        StartCoroutine(CreateCube(wait));
    }

    private IEnumerator CreateCube(WaitForSeconds wait)
    {
        //计算侧面顶点
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < length; j++)
            {
                vertices[curVertexIndex++] = new Vector3(j, i, 0);
                yield return wait;
            }
            for (int k = 0; k < width; k++)
            {
                vertices[curVertexIndex++] = new Vector3(length - 1, i, k);
                yield return wait;
            }
            for (int j = 0; j < length; j++)
            {
                vertices[curVertexIndex++] = new Vector3(length - 1 - j, i, width - 1);
                yield return wait;
            }
            for (int k = 0; k < width; k++)
            {
                vertices[curVertexIndex++] = new Vector3(0, i, width - 1 - k);
                yield return wait;
            }
        }

        //计算上面顶点
        yield return VerticeOnHorizontalPlane(height - 1, curVertexIndex,wait);

        //计算下面顶点
        yield return VerticeOnHorizontalPlane(0, curVertexIndex, wait);
    }

    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        Gizmos.color = Color.black;
        for (int i = 0; i < totalVerticesCount; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.2f);
        }
    }

    /// <summary>
    /// 计算顶层/底层 面上的顶点（不包括边框）
    /// </summary>
    /// <param name="height">水平面高度</param>
    /// <param name="startIndex">开始Index</param>
    private IEnumerator VerticeOnHorizontalPlane(int height, int startIndex,WaitForSeconds wait)
    {
        for (int i = 1; i < width - 1; i++)
        {
            for (int j = 1; j < length - 1; j++)
            {
                vertices[startIndex++] = new Vector3(j, height, i);
                yield return wait;
            }
        }
        this.curVertexIndex = startIndex;
    }
}
